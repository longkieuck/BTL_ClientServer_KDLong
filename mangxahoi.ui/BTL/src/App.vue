<template>
  <div id="app">
    <router-view/>
  </div>  
</template>

<script>
// import NewFeed from './components/NewFeed.vue'
import { mapState } from "vuex";
export default {
  name: 'App',
  components: {
    // NewFeed
  },
  created(){
  },
  computed:{
    ...mapState({
      user: (state) => state.user.user,
      webSocket:(state)=> state.user.webSocket
    }),
  },
  beforeDestroy(){
    if(this.user.Id){
      this.webSocket.disconnect()
    }
  }
}
</script>

<style>
@import "./assets/css/common/common.css";
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
::-webkit-scrollbar {
    width: 10px;
    height: 10px;
}
input::placeholder {
    font-style: italic;
    color: #c4c4c4;
    font-weight: 400;
}
 textarea::placeholder {
    font-style: italic;
    color: #c4c4c4;
    font-weight: 100;
}
body{
  margin: 0;
  color: #111 !important;
}
input{
  line-height: normal !important;
}
textarea::-webkit-scrollbar {
    width: 8px;
}

textarea::-webkit-scrollbar-thumb {
  background-color: darkgrey;
  border-radius: 20px;
  background-clip: padding-box; 
}
textarea:focus,
input:focus,
textarea:hover,
input:hover{
    border: 1px solid #005abd;
}
</style>
