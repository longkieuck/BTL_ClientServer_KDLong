import Login from '../src/components/Login.vue';
import NewFeed from '../src/components/NewFeed.vue'
import ForgotPassword from '../src/components/ForgotPassword.vue';
import Profile from '../src/components/Profile.vue';
import Verify from '../src/components/Verify.vue';
import ChangePassword from '../src/components/ChangePassword.vue';
import CreatePassword from '../src/components/CreatePassword.vue';
import Search from '../src/components/Search.vue';
import Admin from '../src/components/Admin.vue';
import VueRouter from 'vue-router';
const routes = [{
        path: '/',
        redirect: '/login'
    },
    { path: '/login', component: Login },
    { path: '/forgotpassword', component: ForgotPassword },
    { path: '/newfeed', component: NewFeed },
    { name: 'profile', path: '/profile/:id?', component: Profile },
    { path: '/verify', component: Verify },
    { path: '/changepassword', component: ChangePassword },
    { path: '/createpassword', component: CreatePassword },
    { path: '/search', component: Search },
    { path: '/admin', component: Admin },
]


export default new VueRouter({
    routes
})