<template>
  <div>
    <div class="select-box">
      <el-form :inline="true" :model="selectData" class="demo-form-inline">
        <el-form-item label="标题">
          <el-input v-model="selectData.title" placeholder="请输入关键字" clearable/>
        </el-form-item>
        <el-form-item label="详情">
          <el-input v-model="selectData.introduce" placeholder="请输入关键字" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="onSubmit">查询</el-button>
        </el-form-item>
      </el-form>
    </div>
    <el-table :data="dataList.comList" border style="width: 100%">
      <el-table-column prop="id" label="ID" width="180" />
      <el-table-column prop="title" label="标题" width="180" />
      <el-table-column prop="introduce" label="详情" />
    </el-table>
    <el-pagination @current-change="currentChange" @size-change="sizeChange" layout="prev, pager, next"
      :total="selectData.count * 2" />
  </div>
</template>

<script lang="ts">
import {
  computed,
  defineComponent,
  onMounted,
  reactive,
  toRefs,
  watch,
} from "vue";
import { getGoodsList } from "../request/api";
import { InitData, ListInt } from "../type/goods";
export default defineComponent({
  setup() {
    const data = reactive(new InitData());
    onMounted(() => {
      getGoods();
    });
    const getGoods = async() => {
       try {
        const res = await getGoodsList();
        if (res && res.data) {
          // 确保 res 和 res.data 都存在
          data.list = res.data;
          data.selectData.count = res.data.length;
          console.log(res);
        }
      } catch (error) {
        console.error("Error fetching goods:", error);
      }
    };
    const dataList = reactive({
      comList: computed(() => {
        // 1-->[1-10]
        // 2-->[11-20]
        // 3-->[21-30]
        // ...
        return data.list.slice(
          (data.selectData.page - 1) * data.selectData.pagesize, //page=1 -->0,page=2-->10
          data.selectData.page * data.selectData.pagesize //page=1-->10,page=2-->20
          //   0-9
          // 10-19
        );
      }),
    });
    const currentChange = (page: number) => {
      data.selectData.page = page;
    };
    const sizeChange = (pagesize: number) => {
      data.selectData.pagesize = pagesize;
    };
    // 查询
    const onSubmit = () => {
      // console.log(data.selectData.title);
      // console.log(data.selectData.introduce);
      let arr: ListInt[] = []; //定义数组，用来接受查询过后要展示的数据
      if (data.selectData.title || data.selectData.introduce) {
        //判断两个是否其中一个有值
        if (data.selectData.title) {
          arr = data.list.filter((value) => {
            //将过滤出来数组赋值给arr
            return value.title.indexOf(data.selectData.title) !== -1;
          });
        }
        if (data.selectData.introduce) {
          arr = data.list.filter((value) => {
            //将过滤出来数组赋值给arr
            return value.introduce.indexOf(data.selectData.introduce) !== -1;
          });
        }
      } else {
        arr = data.list;
      }
      data.selectData.count = arr.length;
      data.list = arr;
    };
    // 监听输入框的两个属性
    watch(
      [() => data.selectData.title, () => data.selectData.introduce],
      () => {
        if (data.selectData.title == "" && data.selectData.introduce == "") {
          getGoods();
        }
      }
    );
    return { ...toRefs(data), currentChange, sizeChange, dataList, onSubmit };
  },
});
</script>

<style scoped></style>