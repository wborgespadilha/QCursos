<script setup>
import moment from "moment";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Suas avaliações</h1>
    <hr />

    <!-- Modal add test -->
    <div
      class="modal fade"
      id="addTestModal"
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
              Adicionar nova avaliação
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
                  v-model="newTest.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="date"
                  class="form-control transparent"
                  aria-describedby="basic-addon1"
                  v-model="newTest.test_date"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <select class="form-select transparent" v-model="newTest.type">
                  <option
                    class="text-dark"
                    v-for="_type in allTypes"
                    :value="_type.id"
                  >
                    {{ _type.name }}
                  </option>
                </select>
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerTest()"
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

    <!-- Modal select test -->
    <div
      class="modal fade"
      id="selectTestModal"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">Avaliação</h5>
          </div>
          <div class="modal-body bg-dark">
            <h1></h1>
            <table class="table table-striped overFlow">
              <thead>
                <tr>
                  <th>Estudante</th>
                  <th>Nota</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(test, pos) in allAppliedTests">
                  <td>
                    {{ allStudents.find((s) => s.id == test.fk_student).name }}
                  </td>
                  <td>
                    <input
                      class="form-control transparent"
                      type="number"
                      v-model="allAppliedTests[pos].grade"
                      :placeholder="allAppliedTests[pos].grade"
                    />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              class="btn btn-outline-success"
              v-on:click="saveStudentGrade()"
            >
              Salvar
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

    <!-- Course and class selector -->
    <div>
      <form>
        <div class="row overFlow">
          <div class="col overFlow">
            <h4>Curso</h4>
            <select class="form-select transparent" v-model="currentCourse">
              <option
                class="text-dark"
                v-for="course in allCourses"
                :value="course.id"
              >
                {{ course.name }}
              </option>
            </select>
          </div>
          <div class="col">
            <h4>Turma</h4>
            <select class="form-select transparent" v-model="currentClass">
              <option
                class="text-dark"
                v-for="_class in allClasses.filter(
                  (c) => c.fk_course == currentCourse
                )"
                :value="_class.id"
              >
                {{ _class.name }}
              </option>
            </select>
          </div>
          <div class="col">
            <h4>&zwnj;</h4>
            <!-- Button trigger add test -->
            <button
              type="button"
              class="btn btn-outline-primary"
              data-bs-toggle="modal"
              data-bs-target="#addTestModal"
              style="width: 100%"
            >
              Adicionar nova avaliação
            </button>
          </div>
        </div>
      </form>
      <hr />
      <div class="row overFlow">
        <!-- Table with tests -->
        <table class="table table-striped overFlow">
          <thead>
            <tr>
              <th>Avaliação</th>
              <th>Data de aplicação</th>
              <th>Tipo</th>
              <th style="text-align: right">Ação</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="test in allTests.filter((t) => t.fk_class == currentClass)"
            >
              <td>{{ test.name }}</td>
              <td>{{ moment(test.test_Date).format("DD/MM/YYYY") }}</td>
              <td>{{ allTypes.find((t) => t.id == test.fk_type)?.name }}</td>
              <td style="text-align: right">
                <!-- Button trigger select test -->
                <button
                  class="btn btn-outline-info btn-m"
                  data-bs-toggle="modal"
                  data-bs-target="#selectTestModal"
                  v-on:click="selectTests(test.id)"
                >
                  Notas
                </button>
                <button

                  class="btn btn-outline-danger btn-m"
                  v-on:click="deleteTest(test.id)"
                >
                  Excluir
                </button>
              </td>
            </tr>
          </tbody>
        </table>
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
  /* border: 1px solid red; */
  margin-top: 25px;
}
.btn-m {
  margin: 0.25rem;
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
  name: "TestsComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );

    await this.getEverything();
  },
  data() {
    return {
      currentLogged: {},
      allClasses: [],
      allStudents: [],
      allTests: [],
      allTypes: [],
      allCourses: [],
      allStudentClasses: [],
      allAppliedTests: [],
      currentClass: undefined,
      currentCourse: undefined,
      currentTest: undefined,
      newTest: {
        name: "",
        test_date: "",
        type: -1,
      },
      newGrade: 0,
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async saveStudentGrade() {
      const response = await api.put(`/AppliedTests`, this.allAppliedTests);

      this.resultMessage = await response.data;
      await this.showMessage();

      await this.getEverything();
    },
    async getEverything() {
      await this.getAllClasses();
      await this.getAllCourses();
      await this.getAllStudentClasses();
      await this.getAllStudents();
      await this.getAllTests();
      await this.getAllTypes();
    },
    async selectTests(id) {
      this.currentTest = id;
      await this.getAllAppliedTests(id);
    },
    async getAllTypes() {
      this.allTypes = (await api.get("/TestsTypes")).data;
    },
    async getAllAppliedTests(id) {
      this.allAppliedTests = (
        await api.get(
          `/AppliedTests/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      )?.data.filter((test) => test.fk_test == id);
    },
    async getAllClasses() {
      this.allClasses = (
        await api.get(`/Classes/Teacher?Teacher=${this.currentLogged.id}`)
      ).data;
    },
    async getAllStudents() {
      this.allStudents = (
        await api.get(
          `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllStudentClasses() {
      this.allStudentClasses = (
        await api.get(
          `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllCourses() {
      this.allCourses = (
        await api.get(
          `/Courses/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllTests() {
      this.allTests = (
        await api.get(`/Tests/Teacher?Teacher=${this.currentLogged.id}`)
      ).data;
    },
    async registerTest() {
      await api.post("/Tests", {
        name: this.newTest.name,
        test_Date: this.newTest.test_date,
        fk_type: this.newTest.type,
        fk_class: this.currentClass,
      });
      await this.getEverything();
    },
    async deleteTest(id) {
      await api.delete(`/Tests/${id}`);
      await this.getEverything();
    },
  },
};
</script>
