Vue.prototype.$eventHub = new Vue();

var crudMixin = {
    methods: {
        updateResource(resource, type) {
            var self = this;
            fetch(`/api/${type}/${resource.id}`, {
                method: 'PUT',
                credentials: 'same-origin',
                body: JSON.stringify(resource),
                headers: {
                    'Content-Type': 'application/json'

                }
            }).then(function (res) {
                if (res.ok) {
                    console.log(`Saved changes for resource ${type}-${resource.id}.`);
                    
                } else {
                    console.warn("Changes rejected by server.");
                }

                if (typeof self.isInvalid !== "undefined") {
                    self.isInvalid = !res.ok;
                }
            });
        },
        deleteResource(resourceId, type) {
            if (confirm("Are you sure you want to delete this resource?")) {
                var self = this;

                fetch(`/api/${type}/${resourceId}`, {
                    method: 'DELETE',
                    credentials: 'same-origin',
                    headers: {
                        'Content-Type': 'application/json'

                    }
                }).then(function (res) {
                    if (res.ok) {
                        console.log("Resource deleted!");

                        self.$eventHub.$emit('resource-change' + type);

                    } else {
                        console.warn("Error deleting resource");
                    }
                });
            }
        },

        createResource(resource, type) {
            var self = this;

            fetch(`/api/${type}`, {
                method: 'POST',
                credentials: 'same-origin',
                body: JSON.stringify(resource),
                headers: {
                    'Content-Type': 'application/json'

                }
            }).then(function (res) {
                if (res.ok) {
                    console.log("Resource created!");

                    self.$eventHub.$emit('resource-change' + type);

                    if (typeof self.createErrorMessage !== "undefined") {
                        self.createErrorMessage = '';
                    }

                    if (type == "Items") {
                        self.model = {
                            name: "",
                            description: "",
                            quantityAvailable: "",
                            purchasePrice: "",
                            salePrice: "",
                            imagePath: ""
                        }
                    }

                } else {
                    console.warn("Error creating resource");
                    res.json().then(function (data) {
                        if (typeof self.createErrorMessage !== "undefined") {
                            self.createErrorMessage = `Failed to create resource: ${JSON.stringify(data)}`;
                        }
                    });                    
                }
            });
        }
    }
}

Vue.component('sale-point', {
    props: ['type'],
    data: function () {
        return {
            availableItems: [],
            items: [],
            selector: {
                item: null,
                quantity: ""
            },
            meta: "",
            orderFailed: false,
            orderMessage: ""
        }
    },
    mounted: function () {
        this.fetchItems();
    },
    created: function () {
        this.$eventHub.$on('resource-changeItems', this.fetchItems);
    },
    beforeDestroy() {
        this.$eventHub.$off('resource-changeItems');
    },
    computed: {
        total: function () {
            if (this.items.length == 0) {
                return 0;
            }

            var sum = 0;

            this.items.forEach(function (item) {
                sum += parseFloat(item.price) * parseInt(item.quantity);
            });

            return sum;
        },
        typeHeader: function () {
            return this.type.replace(/([A-Z])/g, ' $1').trim().slice(0, -1);
        }
    },
    methods: {
        fetchItems: function () {
            var self = this;

            fetch('/api/Items', {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.availableItems = res;
            });
        },
        removeItem: function (index) {
            this.items.splice(index, 1);
        },
        addItem: function () {
            this.orderMessage = "";
            this.orderFailed = false;
            if (this.selector.item == null || parseInt(this.selector.quantity <= 0)) {
                this.orderMessage = "Choose an item with a quantity greater than 0 to continue.";
                this.orderFailed = true;
            }
            var preItem = this.selector.item,
                price = 0,
                qty = parseInt(this.selector.quantity),
                indexOfItem = -1;

            if (this.type == 'SaleOrders') {
                price = this.selector.item.salePrice;
            } else {
                price = this.selector.item.purchasePrice;
            }

            if (this.type == 'SaleOrders' && qty > preItem.quantityAvailable) {
                var message = "";
                qty = preItem.quantityAvailable;
                switch (qty) {
                    case 0:
                        message = `There are no ${preItem.name}`;
                        break;
                    case 1:
                        message = `There is only one ${preItem.name}`;
                        break;
                    default:
                        message = `There are only ${qty} ${preItem.name}s`;
                }

                alert(`${message} available in stock. Updating quantity.`);
            }

            preItem['price'] = price;
            preItem['quantity'] = qty;

            indexOfItem = this.items.findIndex(function (item) { return item.id == preItem.id });

            if (indexOfItem != -1) {
                this.items[indexOfItem].quantity = parseInt(this.selector.quantity);
            } else {
                this.items.push(preItem);   
            } 

            this.selector.item = null;
            this.selector.quantity = "";
        },
        placeOrder: function () {
            if (this.items.length == 0) {
                this.orderMessage = "Please add at least one item to the order";
                this.orderFailed = true;
                return;
            }

            var relationshipArray = [];
            var requestObject = {};
            var self = this;
            var fetchUrl = null;

            if (this.type == "SaleOrders") {
                this.items.forEach(function (item) {
                    relationshipArray.push({
                        ItemEntityId: item.id,
                        SoldQuantity: item.quantity,
                        SoldPrice: item.salePrice
                    });
                });

                requestObject['SoldItems'] = relationshipArray;
                requestObject['ClientName'] = this.meta;
                fetchUrl = "/api/SaleOrders";
            } else {
                this.items.forEach(function (item) {
                    relationshipArray.push({
                        ItemEntityId: item.id,
                        PurchasedQuantity: item.quantity,
                        PurchasedPrice: item.purchasePrice
                    });
                });

                requestObject['PurchasedItems'] = relationshipArray;
                requestObject['Description'] = this.meta;
                fetchUrl = "/api/PurchaseOrders";
            }

            fetch(fetchUrl, {
                method: 'POST',
                credentials: 'same-origin',
                body: JSON.stringify(requestObject),
                headers: {
                    'Content-Type': 'application/json'

                }
            }).then(function (res) {
                if (res.ok) {
                    console.log(self.type, "created!");

                    self.$eventHub.$emit('resource-change' + self.type);
                    self.$eventHub.$emit('resource-changeItems');

                    self.orderMessage = "Order succesfully created!";
                    self.orderFailed = false;

                    setTimeout(function () {
                        self.meta = "";
                        self.items = [];
                        self.orderMessage = "";
                    }, 3000);

                } else {
                    console.warn("Error creating order");
                    res.json().then(function (data) {
                        self.orderMessage = `Error creating order: ${JSON.stringify(data)}`;
                        self.orderFailed = true;
                    });
                }
            });
        }
    },
    template: `<div class='sale-point'>
                <h2>New {{ typeHeader }}</h2>
                <input class='main' placeholder='Client Name' v-if="type=='SaleOrders'" v-model="meta" />
                <input class='main' placeholder='Description' v-else v-model="meta" />
                <h3>Items</h3>
                <div class='faux head'>
                    <slot></slot>
                </div>
                <div class='faux row' v-for="(item, index) in items">
                    <img :src="item.imagePath" alt="Item Image Missing"/>
                    <p>{{ item.name }}</p>
                    <p>{{ item.description }}</p>
                    <p>{{ item.quantity }}</p>
                    <p>$\{{ item.price }}</p>
                    <p>$\{{ item.quantity * item.price }}</p>
                    <p class="glyphicon glyphicon-remove" v-on:click="removeItem(index)"></p>
                </div>
                <div class='item-adder'>
                    <select v-model="selector.item">
                        <option :value="null" disabled>Select an Item</option>
                        <option v-for="item in availableItems" :value="item">{{ item.name }}</option>
                    </select>
                    <input type="number" v-model="selector.quantity" placeholder="Quantity">
                    <p class="glyphicon glyphicon-plus" v-on:click="addItem()"></p>                    
                </div>
                <div class="finisher">
                    <p><b>Order Total:</b> $\{{ total }}</p>
                    <button v-on:click="placeOrder()">Place Order</button>
                </div>
                <div class="order-message" v-bind:class='{ failed: orderFailed }'>
                 {{ orderMessage }}
                </div>
               </div>`
});

Vue.component('simple-crud', {
    props: ['resourcetype'],
    data: function () {
        return {
            resourceCollection: {}
        }
    },
    created: function () {
        this.$eventHub.$on('resource-change' + this.resourcetype, this.fetchData);
    },
    mounted: function () {
        this.fetchData();
    },
    beforeDestroy() {
        this.$eventHub.$off('resource-change' + this.resourcetype);
    },
    methods: {
        fetchData: function() {
            var self = this;

            fetch(`/api/${self.resourcetype}`, {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.resourceCollection = res;
            });
        }
    },
    template: `<div class='simple-crud'>
                <div class='faux head'>
                    <slot></slot>
                </div>
                <div class='crud-row' v-for='r in resourceCollection'>
                    <item-crud-row v-if="resourcetype=='Items'" v-bind:key="r.id" v-bind:item="r">                    
                    </item-crud-row>
                    <purchase-crud-row v-if="resourcetype=='PurchaseOrders'" v-bind:key="r.id" v-bind:order="r">                    
                    </purchase-crud-row>
                    <sale-crud-row v-if="resourcetype=='SaleOrders'" v-bind:key="r.id" v-bind:order="r">                    
                    </sale-crud-row>
                </div>
                <div class='create-row'>
                    <item-create-row v-if="resourcetype=='Items'"></item-create-row>
                </div>
               </div>`
});

Vue.component('sale-crud-row', {
    props: ['order'],
    mixins: [crudMixin],
    data: function () {
        return {
            isInvalid: false
        }
    },
    template: `<div class='faux row' v-bind:class='{ invalid: isInvalid }'>
                <p>{{ order.id }}</p>
                <input v-model="order.dateOfSale" v-on:change="updateResource(order, 'SaleOrders')"/>
                <input v-model="order.clientName" v-on:change="updateResource(order, 'SaleOrders')"/>
                <div class="item-visualizer">
                    <div class="item-wrapper" v-for="item in order.soldItems">
                        <p><b>{{ item.item.name }}</b></p>
                        <div class="item-details">
                            <p><b>Quantity:</b> {{ item.soldQuantity }}</p>
                            <p><b>Sale $:</b> $\{{ item.soldPrice }}</p>
                        </div>
                    </div>
                </div>
                <p>$\{{ order.total }}</p>
                <p class="glyphicon glyphicon-remove" v-on:click="deleteResource(order.id, 'SaleOrders')"></p>
               </div>`
});

Vue.component('purchase-crud-row', {
    props: ['order'],
    mixins: [crudMixin],
    data: function () {
        return {
            isInvalid: false
        }
    },
    template: `<div class='faux row' v-bind:class='{ invalid: isInvalid }'>
                <p>{{ order.id }}</p>
                <input v-model="order.dateOfPurchase" v-on:change="updateResource(order, 'PurchaseOrders')"/>
                <input v-model="order.description" v-on:change="updateResource(order, 'PurchaseOrders')"/>
                <div class="item-visualizer">
                    <div class="item-wrapper" v-for="item in order.purchasedItems">
                        <p><b>{{ item.item.name }}</b></p>
                        <div class="item-details">
                            <p><b>Quantity:</b> {{ item.purchasedQuantity }}</p>
                            <p><b>Purch. $:</b> $\{{ item.purchasedPrice }}</p>
                        </div>
                    </div>
                </div>
                <p>$\{{ order.total }}</p>
                <p class="glyphicon glyphicon-remove" v-on:click="deleteResource(order.id, 'PurchaseOrders')"></p>
               </div>`
});

Vue.component('item-crud-row', {
    props: ['item'],
    mixins: [crudMixin],
    data: function () {
        return {
            isInvalid: false
        }
    },
    template: `<div class='faux row' v-bind:class='{ invalid: isInvalid }'>
                <p>{{ item.id }}</p>
                <p>{{ item.productCode }}</p>
                <input v-model="item.name" v-on:change="updateResource(item, 'Items')"/>
                <input v-model="item.description" v-on:change="updateResource(item, 'Items')"/>
                <p>{{ item.quantityAvailable }}</p>
                <input type="number" v-model="item.purchasePrice" v-on:change="updateResource(item, 'Items')"/>
                <input type="number" v-model="item.salePrice" v-on:change="updateResource(item, 'Items')"/>
                <input v-model="item.imagePath" v-on:change="updateResource(item, 'Items')"/>
                <p class="glyphicon glyphicon-remove" v-on:click="deleteResource(item.id, 'Items')"></p>
               </div>`
});

Vue.component('item-create-row', {
    data: function () {
        return {
            model: {
                name: "",
                description: "",
                quantityAvailable: "",
                purchasePrice: "",
                salePrice: "",
                imagePath: ""
            },
            createErrorMessage: ""
        }
    },
    mixins: [crudMixin],
    template: `<div><div class='faux row create'>
                <p>
                <p>New Resource:</p>               
                <input placeholder="Name" v-model="model.name"/>
                <input placeholder="Description" v-model="model.description"/>
                <input placeholder="Qty" type="number" v-model="model.quantityAvailable"/>
                <input placeholder="Purch. $" type="number" v-model="model.purchasePrice"/>
                <input placeholder="Sale $" type="number" v-model="model.salePrice"/>
                <input placeholder="https://super-images.com/img.jpg" v-model="model.imagePath"/>
                <p class="glyphicon glyphicon-plus" v-on:click="createResource(model, 'Items')"></p>
               </div>
                <div class='create-error'>{{ createErrorMessage }}</div></div>`
});

var app = new Vue({
    el: '#app',
    mounted: function () {
        this.verifyAdminAccess();
        this.getItemReports();
    },
    data: {
        canAccessAdmin: false,
        reports: {
            profits: {
                delta: 7,
                data: {
                    incomes: [],
                    expenses: [],
                    profit: {}
                }
            },

            items: {
                popular: {
                    delta: 7,
                    data: []
                },

                lowStock: {
                    delta: 7,
                    data: []
                }
            }
        }
    },
    methods: {
        verifyAdminAccess: function () {
            var self = this;

            fetch("/api/Dashboard/authorize", {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.canAccessAdmin = res;

                if (self.canAccessAdmin) {
                    self.getProfitReports();
                }
            });

        },

        getProfitReports: function () {
            var self = this;

            fetch(`/api/Dashboard/profits/${self.reports.profits.delta}`, {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.reports.profits.data.incomes = res.incomes;
                self.reports.profits.data.expenses = res.expenses;
                self.reports.profits.data.profit = res.profit;
            });
        },

        getItemReports: function () {
            var self = this;

            fetch(`/api/Dashboard/popular/${self.reports.items.popular.delta}`, {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.reports.items.popular.data = res;
            });

            fetch(`/api/Dashboard/lowstock/${self.reports.items.lowStock.delta}`, {
                credentials: 'same-origin'
            }).then(function (res) {
                return res.json();
            }).then(function (res) {
                self.reports.items.lowStock.data = res;
            });
        }
    }
});
