import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import HomeView from "../views/HomeView.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
    redirect: "caseStatistics", //设置重定向
    children: [
      {
        path: "caseStatistics",
        name: "caseStatistics",
        meta: {
          isShow: true,
          title: "案件统计",
        },
        component: () =>
          import("../views/CaseStatistics.vue"),
      },
      {
        path: "user",
        name: "user",
        meta: {
          isShow: true,
          title: "执法人员管理",
        },
        component: () => import("../views/PoliceView.vue"),
      },
      {
        path: "lawType",
        name: "lawType",
        meta: {
          isShow: true,
          title: "法律条文类型管理",
        },
        component: () => import("../views/LawType.vue"),
      },
      {
        path: "Law",
        name: "Law",
        meta: {
          isShow: true,
          title: "法律条文管理",
        },
        component: () => import("../views/LawView.vue"),
      },
      {
        path: "Case",
        name: "Case",
        meta: {
          isShow: true,
          title: "案件管理",
        },
        component: () => import("../views/CaseView.vue"),
      },
      {
        path: "authority",
        name: "authority",
        meta: {
          isShow: false,
          title: "权限列表",
        },
        component: () =>
          import(
            /* webpackChunkName: "authority" */ "../views/AuthorityView.vue"
          ),
      },
    ],
  },
  {
    path: "/login",
    name: "login",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "login" */ "../views/LoginView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});
// router.beforeEach((to,from,next)=>{
//   const token: string | null =localStorage.getItem('token')
//   if(!token && to.path!=='/login'){
//     next('/login')
//   }else{
//     next()
//   }
// })
export default router;
