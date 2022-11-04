<template>
  <main class="main bg-darker">
    <!-- Navbar -->
    <nav
      class="navbar navbar-expand-lg navbar-dark transparentDark"
      style="position: sticky; top: 0; z-index: 1000"
    >
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
                v-on:click="changeComponent('institutions')"
                class="nav-link"
                aria-current="page"
                href="#"
                >Instituições</a
              >
            </li>
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('admins')"
                class="nav-link"
                href="#"
                >Admins</a
              >
            </li>
            <li class="nav-item undercolor">
              <a
                v-on:click="changeComponent('testTypes')"
                class="nav-link"
                href="#"
                >Tipos de avaliação</a
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

    <div v-if="actualComponent == 'institutions'" class="container">
      <InstitutionsComponent />
    </div>
    <div v-if="actualComponent == 'admins'" class="container">
      <AdminsComponent />
    </div>
    <div v-if="actualComponent == 'testTypes'" class="container">
      <TestTypesComponent />
    </div>
  </main>
</template>

<style scoped>
.main {
  min-height: 100vh;

  background-image: url("../../src/assets/background2.jpg");
  background-blend-mode: multiply;
  background-size: 100%;
}
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
import InstitutionsComponent from "../components/MasterComponents/InstitutionsComponent.vue";
import AdminsComponent from "../components/MasterComponents/AdminsComponent.vue";
import TestTypesComponent from "../components/MasterComponents/TestTypesComponent.vue";
import api from "../services/api";

export default {
  name: "MasterView",
  components: { InstitutionsComponent, AdminsComponent, TestTypesComponent },
  data() {
    return {
      actualComponent: "institutions",
    };
  },
  beforeMount() {
    this.verifyPermission();
    this.actualComponent = "institutions";
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

        const response = await api.post("/Authentication/loginMADM", {
          registry: user.registry,
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
    logout() {
      localStorage.removeItem("@q-cursos:user");
      this.$router.push("/");
    },
    async changeComponent(target) {
      this.actualComponent = target;
    },
  },
};
</script>
