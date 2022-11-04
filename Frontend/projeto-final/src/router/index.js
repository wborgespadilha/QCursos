import { createRouter, createWebHistory } from "vue-router";
import LoginView from "../views/LoginView.vue";
import TeacherView from "../views/TeacherView.vue";
import StudentView from "../views/StudentView.vue";
import AdminView from "../views/AdminView.vue";
import MasterView from "../views/MasterView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "login",
      component: LoginView,
    },
    {
      path: "/dGVhY2hlcl9tZW51",
      name: "teacher-menu",
      component: TeacherView,
    },
    {
      path: "/c3R1ZGVudF9tZW51",
      name: "student-menu",
      component: StudentView,
    },
    {
      path: "/YWRtaW5fbWVudQ",
      name: "admin-menu",
      component: AdminView,
    },
    {
      path: "/bWFzdGVyX21lbnU",
      name: "master-menu",
      component: MasterView,
    },
  ],
});

export default router;
