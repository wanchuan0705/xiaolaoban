import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import VueECharts  from 'vue-echarts'
import "echarts";
// 如果您正在使用CDN引入，请删除下面一行。
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

// 创建应用
const app = createApp(App);

for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component)
}
// 使用路由
app.use(router);

// 使用 Element Plus
app.use(ElementPlus);

// 全局注册组件（也可以使用局部注册）
app.component('Echarts', VueECharts)
// 挂载应用到 DOM 元素
app.mount('#app');
