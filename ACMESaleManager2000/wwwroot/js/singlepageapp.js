Vue.component('simple-crud', {
    props: ['resourcetype'],
    data: function () {
        return {
            resourceCollection: {}
        }
    },
    mounted: function () {
        var self = this;

        fetch(`/api/${self.resourcetype}`, {
            credentials: 'same-origin'
        }).then(function (res) {
            return res.json();
        }).then(function (res) {
            self.resourceCollection = res;
        });
    },
    template: `<div class='simple-crud'>
                <div class='crud-row' v-for='r in resourceCollection'>

                </div>
                <div class='create-row'>
    
                </div>
               </div>`
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
