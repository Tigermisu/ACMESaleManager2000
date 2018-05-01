Vue.component('simple-crud', {
    props: ['resource'],
    data: function () {
        return {
            count: 0
        }
    },
    template: '<button v-on:click="count++">You clicked me {{ count }} times. I am a {{ resource }}</button>'
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
                console.log("Authorized call result", res);
                self.canAccessAdmin = res;
            });

        }
    }
});

var itemManagement = new Vue({
    el: "#itemManagement",
    data: {
        items: [
            { id: 23, description: "A big boollet" },
            { id: 10, description: "Toilet flushing mechanism" }
        ]
    }

});
