<template>
  <div>
    <div class="input-search1">
      <input
        v-bind="$attrs"
        type="text"
        class="input"
        :style="{width:lWidth +'px' }"
        v-on="inputSearchListeners"
      />
      <div class="icon-search"></div>
    </div>
  </div>
</template>

<script>
export default {
    props:{
        lWidth:String//Độ rộng
    },
    computed: {
        /**
         * Customize event(Merge)
         * CreatedBy KDLong 30/05/2021
         */
        inputSearchListeners: function () {
            var vm = this
            return Object.assign({},
                this.$listeners,//Các sự kiện có sẵn
                {
                input: function (event) {
                    // Sự kiện overide
                    vm.$emit('input', event.target.value)
                }
                }
            )
        }
    }
}
</script>

<style>
.input-search1 {
  display: flex;
}
.input {
  border-radius: 2px;
  font-size: 13px;
  padding: 6px 10px;
  padding-right: 30px;
  outline: none;
  height: 32px;
  box-sizing: border-box;
  border: 1px solid #babec5;
}
.icon-search {
  background-image: url("../assets/img/icon1.svg");
  background-position: -992px -360px;
  width: 16px;
  height: 16px;
  min-width: 16px;
  min-height: 16px;
  margin-right: -10px;
  position: relative;
  top: 8px;
  right: 25px;
}

</style>