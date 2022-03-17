import Vue from 'vue'
import App from './App.vue'
//Antd
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
//Router
import VueRouter from 'vue-router'
import router from './router'
//Vue-x
import store from './stores/index'

Vue.use(VueRouter)
Vue.use(Antd)

Vue.config.productionTip = false

new Vue({
    store,
    router,
    render: h => h(App),
}).$mount('#app')