<script setup>
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Instituições</h1>
    <hr />

    <!-- Button trigger add modal -->
    <div class="institutionActions">
      <div class="input-group mb-3">
        <span class="input-group-text" id="basic-addon1">🔎</span>
        <input
          type="text"
          class="form-control transparent"
          v-model="institutionNameFind"
        />
      </div>
      <button
        type="button"
        class="btn btn-outline-primary"
        data-bs-toggle="modal"
        data-bs-target="#addInstitution"
      >
        Adicionar Instituição
      </button>
    </div>

    <!-- Modal add Institution -->
    <div
      class="modal fade"
      id="addInstitution"
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
              Adicionar instituição
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">💬</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  style="color: #000"
                  v-model="newInstituition.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerNewInstituition()"
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

    <!-- Modal edit institutions -->
    <div
      class="modal fade"
      id="alterInstitution"
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
              Editar instituição
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">💬</span>
                <input
                  type="text"
                  class="form-control transparent"
                  placeholder="Novo nome"
                  aria-label="Nome"
                  aria-describedby="basic-addon1"
                  style="color: #000"
                  v-model="newerInstitution.name"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterInstitution()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectInstitution(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Institutions accordion -->
    <div
      class="accordion accordion-flush bg-dark"
      id="accordionFlushExample"
      v-for="institution in allInstituition"
    >
      <div
        class="accordion-item bg-dark"
        v-if="
          institution.name
            ?.toLowerCase()
            .includes(institutionNameFind.toLowerCase())
        "
      >
        <h2 class="accordion-header bg-dark" id="flush-headingOne">
          <button
            class="accordion-button collapsed text-light bg-dark"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="'#institution_' + institution.id"
            aria-expanded="false"
            aria-controls="flush-collapseOne"
          >
            {{ institution.name }}
          </button>
        </h2>
        <div
          :id="'institution_' + institution.id"
          class="accordion-collapse collapse"
          aria-labelledby="flush-headingOne"
        >
          <div class="accordion-body text-light">
            <table class="table">
              <thead>
                <tr>
                  <th>Cursos</th>
                </tr>
              </thead>
              <tbody v-for="course in allCourses">
                <tr v-if="course.fk_institution == institution.id">
                  <td>{{ course.name }}</td>
                </tr>
              </tbody>
              <tfoot class="tableAction">
                <button
                  class="btn btn-outline-warning actionButton"
                  data-bs-toggle="modal"
                  data-bs-target="#alterInstitution"
                  v-on:click="selectInstitution(institution)"
                >
                  Editar
                </button>
                <button
                  class="btn btn-outline-danger actionButton"
                  v-on:click="deleteInstitution(institution.id)"
                >
                  Excluir
                </button>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
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
main {
  margin-top: 25px;
}
.roundTop {
  border-radius: 10px 20px 30px 40px;
}
.left {
  display: flex;
  justify-content: left;
}
.right {
  display: flex;
  justify-content: right;
}
.institutionActions {
  gap: 50px;
  margin: 25px 0;
  width: 100%;
  text-align: left;
}
.tableAction {
  display: flex;
  justify-content: right;
  margin-top: 25px;
}
.actionButton {
  margin-left: 10px;
}
.form-control {
  color: #000 !important;
  font-weight: 600;
}
.title {
  font-weight: 600;
  color: #fff;
}
.add-button {
  margin: 1rem 0;
}
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
.transparent {
  color: #fff !important;
  background-color: transparent;
}
</style>

<script>
export default {
  name: "InstitutionsComponent",
  async beforeMount() {
    await this.getAllInstitution();
    await this.getAllCourses();
  },
  data() {
    return {
      allInstituition: [],
      allCourses: [],
      newInstituition: {
        name: "",
      },
      newerInstitution: {
        name: "",
      },
      institutionNameFind: "",
      currentCourse: [],
      editingInstitutction: -1,
      resultToast: "",
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async alterInstitution() {
      await api.put(`/Institution`, {
        id: this.editingInstitutction,
        name: this.newerInstitution.name,
      });
      await this.getAllInstitution();
      this.newerInstitution.name = "";
    },
    async selectInstitution(institution) {
      this.editingInstitutction = institution.id;
      this.newerInstitution.name = institution.name;
    },
    async getAllInstitution() {
      this.allInstituition = (await api.get("/Institution")).data;
    },
    async getAllCourses() {
      this.allCourses = (await api.get("/Courses")).data;
    },
    async registerNewInstituition() {
      await api.post("/Institution", this.newInstituition);
      await this.getAllInstitution();
      await this.getAllCourses();
    },
    async deleteInstitution(id) {
      const response = api.delete(`Institution/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data;
        await this.showMessage();
      });

      await this.getAllInstitution();
      await this.getAllCourses();
    },
  },
};
</script>
