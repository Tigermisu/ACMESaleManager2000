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
    },
    data: {
        canAccessAdmin: false
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
            });

        }
    }
});
