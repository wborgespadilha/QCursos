<template>
  <main class="main bg-darker">
    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg navbar-dark transparentDark">
      <div class="container-fluid">
        <!-- Navbar Brand -->
        <a class="navbar-brand" href="#">
          <img src="../components/icons/book.png" alt="" /> Q'Cursos
        </a>
        <!-- Navbar Toggler -->
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <!-- Collapse Menu -->
        <!-- Navbar Content -->
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('classes')"
                class="nav-link"
                aria-current="page"
                href="#"
                >Turmas</a
              >
            </li>
            <!-- <li class="nav-item">
              <a
                v-on:click="changeComponent('students')"
                class="nav-link"
                href="#"
                >Alunos</a
              >
            </li> -->
            <li class="nav-item undercolor">
              <a v-on:click="changeComponent('tests')" class="nav-link" href="#"
                >Avaliações</a
              >
            </li>
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('lessons')"
                class="nav-link"
                href="#"
                >Aulas</a
              >
            </li>
            <li class="nav-item">
              <button
                class="logout-button btn btn-outline-danger"
                v-on:click="logout()"
              >
                Sair
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <div v-if="actualComponent == 'classes'" class="container">
      <ClassesComponent />
    </div>
    <div v-if="actualComponent == 'tests'" class="container">
      <TestsComponent />
    </div>
    <div v-if="actualComponent == 'lessons'" class="container">
      <LessonsComponent />
    </div>
  </main>
</template>

<style scoped>
.main {
  min-height: 100vh;

  padding-bottom: 1rem;

  background-image: url("../../src/assets/background2.jpg");
  background-blend-mode: multiply;
  background-size: 100%;
}
/* .navbar {
  background-color: var(--dark-item);
} */
.flex-left {
  display: flex;
  justify-content: left;
  align-items: center;
}
.flex-right {
  display: flex;
  justify-content: right;
  align-items: center;
}
.bg-darker {
  background-color: var(--dark-darker);
}

.logout-button {
  /* margin-left: 1rem; */
  width: 5rem;
}

.nav-item {
  margin: 0 5px;
}
</style>

<script>
import ClassesComponent from "../components/TeacherComponents/ClassesComponent.vue";
import TestsComponent from "../components/TeacherComponents/TestsComponent.vue";
import LessonsComponent from "../components/TeacherComponents/LessonsComponent.vue";
import api from "../services/api";

export default {
  name: "TeacherView",
  components: {
    ClassesComponent,
    TestsComponent,
    LessonsComponent,
  },
  data() {
    return {
      actualComponent: "",
    };
  },
  beforeMount() {
    this.verifyPermission();
    this.actualComponent = "classes";
  },
  methods: {
    async verifyPermission() {
      try {
        let user = localStorage.getItem("@q-cursos:user");

        if (!user) {
          this.$router.push("/");
          return;
        }

        user = JSON.parse(user);

        const response = await api.post("/Authentication/loginTeacher", {
          registry: Number(user.registry),
          password: user.password,
        });

        if (response.data.length == 0) {
          localStorage.clear();
          this.$router.push("/");
          return;
        }
      } catch (error) {
        this.$router.push("/");
      }
    },
    async logout() {
      localStorage.removeItem("@q-cursos:user");
      this.$router.push("/");
    },
    async changeComponent(target) {
      this.actualComponent = target;
    },
  },
};
</script>
