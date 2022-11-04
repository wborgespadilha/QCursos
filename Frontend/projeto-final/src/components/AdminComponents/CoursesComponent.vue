<script setup>
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Cursos disponíveis</h1>
    <hr />

    <!-- Button trigger modal add -->
    <button
      type="button"
      class="btn btn-outline-primary"
      data-bs-toggle="modal"
      data-bs-target="#addCourse"
    >
      Adicionar novo curso
    </button>

    <!-- Modal add course -->
    <div
      class="modal fade"
      id="addCourse"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">
              Adicionar novo curso
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newCourse.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerCourse()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal edit course -->
    <div
      class="modal fade"
      id="alterCourse"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">Editar curso</h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newerCourse.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterCourse()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectCourse(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Table with courses -->
    <table class="table">
      <thead>
        <tr>
          <th>Nome</th>
          <th colspan="2" style="text-align: right">Ação</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="course in allCourses">
          <td>{{ course.name }}</td>
          <td style="text-align: right">
            <button
              class="btn btn-outline-warning"
              data-bs-toggle="modal"
              data-bs-target="#alterCourse"
              v-on:click="selectCourse(course)"
            >
              Editar
            </button>
            <button
              class="btn btn-outline-danger"
              v-on:click="deleteCourse(course.id)"
              style="margin-left: 10px"
            >
              Excluir
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Toast -->
    <div class="toast-container position-fixed bottom-0 end-0 p-3">
      <div
        id="resultToast"
        class="toast bg-dark"
        role="alert"
        aria-live="assertive"
        aria-atomic="true"
      >
        <div class="toast-header bg-dark">
          <strong class="me-auto text-light">📡 Sistema</strong>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="toast"
            aria-label="Close"
          ></button>
        </div>
        <div class="toast-body bg-dark text-light">{{ resultMessage }}</div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.btn {
  margin: 0.25rem;
}
main {
  /* border: 1px solid red; */
  margin-top: 25px;
}
.title {
  font-weight: 600;
  color: #fff;
}
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
</style>

<script>
export default {
  name: "CoursesComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    this.newCourse.fk_institution = this.currentLogged.fk_institution;
    this.newerCourse.fk_institution = this.currentLogged.fk_institution;
    await this.getAllCourses();
  },
  data() {
    return {
      currentLogged: {},
      allCourses: [],

      // copy
      allCourses: [],
      allInstituition: [],
      newCourse: {
        name: "",
        fk_institution: -1,
      },
      newerCourse: {
        name: "",
        fk_institution: -1,
      },
      currentCourse: -1,
      resultMessage: "",
    };
  },
  methods: {
    async getAllCourses() {
      const response = await api.get(
        `/Courses/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allCourses = response.data;
    },
    async deleteCourse(id) {
      await api.delete(`/Courses/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data;
        await this.showMessage();
      });
      await this.getAllCourses();
    },
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async registerCourse() {
      await api.post(`/Courses`, this.newCourse);

      this.getAllCourses();
    },
    async selectCourse(course) {
      this.currentCourse = await course.id;
      this.newerCourse.name = await course.name;
    },
    async alterCourse() {
      const response = await api.put("/Courses", {
        id: this.currentCourse,
        name: this.newerCourse.name,
        fk_institution: this.currentLogged.fk_institution,
      });

      await this.getAllCourses();
      this.currentCourse = -1;
    },
  },
};
</script>
