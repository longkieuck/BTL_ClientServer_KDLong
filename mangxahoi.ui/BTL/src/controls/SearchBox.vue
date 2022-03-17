<template>
    <div class="cover-search-box">
        <input
          class="input-search"
          type="text"
          placeholder="Tìm kiếm người, tin tức được chia sẻ"
          @keyup.enter="search($event)"
          v-model="stringKeyWord"
        />
        <div class="search-icon"></div>
    </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
export default {
    methods:{
        ...mapActions("user", [
            "setStringKeyWord"
        ]),
        search(e){
            this.setStringKeyWord(e.target.value)
            this.$router.push({ path: "/search" });
        }
    },
    data() {
        return {
        };
    },
    computed: {
    ...mapState({
      user: (state) => state.user.user,
      stringKeyWord: (state) => state.user.stringKeyWord,
    }),
  },
}
</script>

<style scoped>
.cover-search-box {
    width: calc(100% - 600px);
    height: 64px;
    display: flex;
    align-items: center;
}
.input-search {
    height: 35px;
    width: 70%;
    margin-left: 50px;
    padding-left: 40px;
    outline: none;
    box-sizing: border-box;
    border: 1px solid #babec5;
    border-radius: 10px;
}
.search-icon {
    background-image: url('../assets/img/icon1.svg');
    min-width: 24px;
    min-height: 24px;
    background-position: -1040px -144px;
    height: 24px;
    width: 24px;
    position: absolute;
    left: 310px;
}
</style>