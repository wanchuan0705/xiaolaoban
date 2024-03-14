<template>
  <div>
    <el-tree
      ref="treeRef"
      :data="list"
      show-checkbox
      node-key="roleId"
      :check-strictly="true"
      :default-checked-keys="authority"
      :props="{
        children: 'roleList',
        label: 'name',
      }"
    />
    <el-button @click="changeAuthority">确认修改</el-button>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, reactive, toRefs } from "vue";
import { useRoute } from "vue-router";
import { InitData } from "../type/authority";
import { getAuthorityList } from "../request/api";
export default defineComponent({
  setup() {
    const route = useRoute();
    console.log(route);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const params: any = route.params;
    const data = reactive(new InitData(params.id, params.authority));
    onMounted(async() => {
      try {
        const res = await getAuthorityList();
        if (res && res.data) {
          // 确保 res 和 res.data 都存在
          console.log(res);
          data.list = res.data;
        }
      } catch (error) {
        console.error("Error fetching goods:", error);
      }
    });
    const changeAuthority = () => {
        console.log(data.treeRef.getCheckedKeys().sort(function(a:number,b:number){return a-b}));
        // 传给后台，后台去进行修改
        
    };
    return { ...toRefs(data), changeAuthority };
  },
});
</script>

<style scoped>
</style>