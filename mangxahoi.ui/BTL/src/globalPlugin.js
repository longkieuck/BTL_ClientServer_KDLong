import Vue from 'vue';
import Enum from './common/enum';
import Auth from './common/auth';

const install = Vue => {
    //prototype
    Vue.prototype.$enum = Enum;
    Vue.prototype.$auth = Auth;
};
install(Vue);
export default install;