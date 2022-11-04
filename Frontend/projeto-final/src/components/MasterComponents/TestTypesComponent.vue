<script setup>
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Tipos de avaliação</h1>
    <hr />

    <!-- Modal add test type -->
    <div
      class="modal fade"
      id="addTestType"
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
              Adicionar tipo de avaliação
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent text-dark"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newTestType.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerTestType()"
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

    <!-- Modal edit test type -->
    <div
      class="modal fade"
      id="alterTestType"
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
              Editar tipo de avaliação
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="text"
                  class="form-control transparent text-dark"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  v-model="newerTestType.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterTestType()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectTestType(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Button trigger add modal -->
    <div style="margin: 25px 0">
      <button
        class="btn btn-outline-primary"
        data-bs-toggle="modal"
        data-bs-target="#addTestType"
      >
        Adicionar tipo de avaliação
      </button>
    </div>

    <!-- Table with test types -->
    <table class="table table-striped">
      <thead>
        <tr>
          <th colspan="3">Nome</th>
        </tr>
      </thead>
      <tbody v-for="testType in allTestTypes">
        <tr>
          <td>{{ testType.name }}</td>
          <td>
            <button
              class="btn btn-outline-warning"
              data-bs-toggle="modal"
              data-bs-target="#alterTestType"
              v-on:click="selectTestType(testType)"
            >
              Editar
            </button>
          </td>
          <td>
            <button
              class="btn btn-outline-danger"
              v-on:click="deleteTestType(testType.id)"
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
  name: "TestTypesComponent",
  async beforeMount() {
    await this.getAllTestTypes();
  },
  data() {
    return {
      allTestTypes: [],
      newTestType: {
        name: "",
      },
      newerTestType: {
        name: "",
      },
      currenttestType: -1,
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async selectTestType(test) {
      this.currenttestType = test.id;
      this.newerTestType.name = test.name;
    },
    async getAllTestTypes() {
      this.allTestTypes = (await api.get("/TestsTypes")).data;
    },
    async deleteTestType(id) {
      await api.delete(`/TestsTypes/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data;
        await this.showMessage();
      });
      this.currenttestType = -1;
      this.getAllTestTypes();
    },
    async alterTestType() {
      await api.put(`/TestsTypes/`, {
        id: this.currenttestType,
        name: this.newerTestType.name,
      });
      this.getAllTestTypes();
      this.newerTestType.name = "";
    },
    async registerTestType() {
      await api.post("/TestsTypes", this.newTestType);
      this.getAllTestTypes();
      this.newTestType.name = "";
    },
  },
};
</script>
