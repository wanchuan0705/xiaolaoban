<template>
    <v-chart class="chart" :option="option" />
</template>

<script  setup lang="ts">
import { use } from "echarts/core";
import { CanvasRenderer } from "echarts/renderers";
import { PieChart } from "echarts/charts";
import {
    TitleComponent,
    TooltipComponent,
    LegendComponent
} from "echarts/components";
import VChart, { THEME_KEY } from "vue-echarts";
import { ref, provide, onMounted, reactive } from "vue";
import axios from "axios";
import { InitData } from "@/type/case";

use([
    CanvasRenderer,
    PieChart,
    TitleComponent,
    TooltipComponent,
    LegendComponent
]);

provide(THEME_KEY, "dark");

const option = ref({
    title: {
        text: "案件进度",
        left: "center"
    },
    tooltip: {
        trigger: "item",
        formatter: "{a} <br/>{b} : {c} ({d}%)"
    },
    legend: {
        orient: "vertical",
        left: "left",
        data: ["未审核", "已审核", "已立案", "已完成", "已结案", "已驳回", "不立案", "已撤销s"]
    },
    series: [
        {
            name: "案件进度",
            type: "pie",
            radius: "55%",
            center: ["50%", "60%"],
            data: [
                { value: 335, name: "未审核" },
                { value: 310, name: "已审核" },
                { value: 234, name: "已立案" },
                { value: 135, name: "已完成" },
                { value: 1548, name: "已结案" },
                { value: 1548, name: "已驳回" },
                { value: 1548, name: "不立案" },
                { value: 1548, name: "已撤销" }
            ],
            emphasis: {
                itemStyle: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: "rgba(0, 0, 0, 0.5)"
                }
            }
        }
    ]
});
onMounted(() => {
    getCase();
});
const getCase = async () => {
    try {
        const data = reactive(new InitData());
        debugger
        const res = await axios.get(`http://localhost:5172/api/Admin/GetCases`);

        // 处理响应
        console.log(res.data); // 假设响应返回数据
        if (res && res.data) {
            // 确保 res 和 res.data 都存在
            data.list = res.data;
            data.selectData.count = res.data.length;
            console.log(data.list);
        }
    } catch (error) {
        console.error("Error fetching goods:", error);
    }
};
</script>

<style scoped>
.chart {
    width: 100%;
    height: 100%;
        /* 设置高度为屏幕的100% */
}
</style>