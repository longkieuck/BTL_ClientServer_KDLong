import Vue from 'vue';
import commonFunc from './common/commonFunc';
import Enum from './common/enum';
import Auth from './common/auth';

const install = Vue => {
    //prototype
    Vue.prototype.$commonFunc = commonFunc;
    Vue.prototype.$enum = Enum;
    Vue.prototype.$auth = Auth;
};
install(Vue);
export default install;