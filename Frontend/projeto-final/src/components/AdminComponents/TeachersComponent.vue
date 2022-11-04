<script setup>
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Professores</h1>
    <hr />

    <!-- Button trigger add teacher -->
    <button
      type="button"
      class="btn btn-outline-primary"
      data-bs-toggle="modal"
      data-bs-target="#addTeacher"
    >
      Adicionar novo professor
    </button>

    <!-- Modal add teacher -->
    <div
      class="modal fade"
      id="addTeacher"
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
              Adicionar novo professor
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="CPF"
                  aria-label="CPF"
                  aria-describedby="basic-addon1"
                  v-model="newTeacher.cpf"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">👤</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newTeacher.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔐</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Password"
                  aria-label="Password"
                  aria-describedby="basic-addon1"
                  v-model="newTeacher.password"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerTeacher()"
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

    <!-- Modal edit teacher -->
    <div
      class="modal fade"
      id="alterTeacher"
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
              Editar professor
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="CPF"
                  aria-label="CPF"
                  aria-describedby="basic-addon1"
                  v-model="newerTeacher.cpf"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">👤</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newerTeacher.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔐</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Senha"
                  aria-label="Senha"
                  aria-describedby="basic-addon1"
                  v-model="newerTeacher.password"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterTeacher()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectTeacher(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Table with teachers -->
    <div class="overflow">
      <table class="table table-striped">
        <thead>
          <tr>
            <th>CPF</th>
            <th>Registro</th>
            <th>Nome</th>
            <th>Senha</th>
            <th>Ação</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="teacher in allTeachers">
            <td>{{ teacher.cpf }}</td>
            <td>{{ teacher.registry }}</td>
            <td>{{ teacher.name }}</td>
            <td class="hidePassword">{{ teacher.password }}</td>
            <td>
              <button
                class="btn btn-outline-warning"
                data-bs-toggle="modal"
                data-bs-target="#alterTeacher"
                v-on:click="selectTeacher(teacher)"
              >
                Editar
              </button>
              <button
                class="btn btn-outline-danger"
                v-on:click="deleteTeacher(teacher.id)"
              >
                Excluir
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

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
.overflow {
  overflow-x: auto;
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
  name: "TeachersComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    await this.getAllTeachers();
  },
  data() {
    return {
      currentLogged: {},
      allTeachers: [],
      newTeacher: {
        name: "",
        cpf: "",
        password: "",
      },
      newerTeacher: {
        name: "",
        cpf: "",
        password: "",
      },
      currentTeacher: -1,
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async getAllTeachers() {
      this.allTeachers = (
        await api.get(
          `/Teachers/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async registerTeacher() {
      await api.post("/Teachers", {
        cpf: this.newTeacher.cpf,
        name: this.newTeacher.name,
        password: this.newTeacher.password,
        fk_institution: this.currentLogged.fk_institution,
      });
      await this.getAllTeachers();
    },
    async deleteTeacher(id) {
      await api.delete(`/Teachers/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data.text;
        await this.showMessage();
      });
      await this.getAllTeachers();
    },
    async selectTeacher(teacher) {
      this.currentTeacher = await teacher.id;
      this.newerTeacher.cpf = await teacher.cpf;
      this.newerTeacher.name = await teacher.name;
      this.newerTeacher.password = await teacher.password;
    },
    async alterTeacher() {
      await api.put("/Teachers", {
        id: this.currentTeacher,
        name: this.newerTeacher.name,
        cpf: this.newerTeacher.cpf,
        password: this.newerTeacher.password,
        fk_institution: this.currentLogged.fk_institution,
      });
      await this.getAllTeachers();
      this.currentTeacher = -1;
    },
  },
};
</script>
