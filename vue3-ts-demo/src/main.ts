import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import VueECharts  from 'vue-echarts'
import "echarts";

// 创建应用
const app = createApp(App);

// 使用路由
app.use(router);

// 使用 Element Plus
app.use(ElementPlus);

// 全局注册组件（也可以使用局部注册）
app.component('Echarts', VueECharts)
// 挂载应用到 DOM 元素
app.mount('#app');
