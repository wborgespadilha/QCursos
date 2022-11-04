<script setup>
import moment from "moment";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Alunos</h1>
    <hr />

    <!-- Modal select student -->
    <div class="modal fade" tabindex="-1" id="menuSelectStudent">
      <div
        class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl"
      >
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title">Turmas do Estudante</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body bg-dark">
            <form class="noOverflow">
              <div class="row">
                <div class="col noOverflow">
                  <h4>Curso</h4>
                  <select
                    class="form-select transparent"
                    v-model="newStudentClassCourse"
                  >
                    <option
                      class="text-dark"
                      v-for="course in allCourses"
                      :value="course.id"
                    >
                      {{ course.name }}
                    </option>
                  </select>
                </div>
                <div class="col noOverflow">
                  <h4>Turma</h4>
                  <select
                    v-model="newStudentClassClass"
                    class="form-select transparent"
                  >
                    <option
                      class="text-dark"
                      v-for="classes in allClasses.filter(
                        (c) => c.fk_course == newStudentClassCourse
                      )"
                      :value="classes.id"
                    >
                      {{ classes.name }}
                    </option>
                  </select>
                </div>
                <div class="col noOverflow">
                  <h4>&#8205;</h4>
                  <button
                    type="button"
                    class="btn btn-outline-info"
                    v-on:click="addStudentToClass()"
                  >
                    Adicionar
                  </button>
                </div>
              </div>
            </form>

            <hr />
            <table class="table table-strip">
              <thead>
                <tr>
                  <th>Curso</th>
                  <th>Turma atual</th>
                  <th>Nova Turma</th>
                  <th>Ação</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="studentClass in studentClasses">
                  <td v-if="studentClass.fk_student == currentStudent">
                    {{ studentClass.course_name }}
                  </td>
                  <td v-if="studentClass.fk_student == currentStudent">
                    {{ studentClass.class_name }}
                  </td>
                  <td v-if="studentClass.fk_student == currentStudent">
                    <select
                      class="form-select transparent"
                      v-model="updateStudentClass"
                    >
                      <option
                        v-for="classes in allClasses.filter(
                          (c) => c.fk_course == studentClass.fk_course
                        )"
                        :value="classes.id"
                        class="text-dark"
                      >
                        {{ classes.name }}
                      </option>
                    </select>
                  </td>
                  <td v-if="studentClass.fk_student == currentStudent">
                    <button
                      class="btn btn-outline-success"
                      v-on:click="
                        saveStudentChanges(studentClass.id, updateStudentClass)
                      "
                    >
                      Salvar
                    </button>
                    <button
                      class="btn btn-outline-danger"
                      v-on:click="deleteStudentClasses(studentClass.id)"
                    >
                      Remover
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectStudent(-1)"
            >
              Sair
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal edit student -->
    <div
      class="modal fade"
      id="alterStudent"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">Editar aluno</h5>
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
                  v-model="newerStudent.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="date"
                  class="form-control transparent"
                  aria-describedby="basic-addon1"
                  v-model="newerStudent.born_date"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="alterStudent()"
            >
              Confirmar
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              data-bs-dismiss="modal"
              v-on:click="selectStudent(-1)"
            >
              Fechar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Button trigger add student -->
    <button
      type="button"
      class="btn btn-outline-primary"
      data-bs-toggle="modal"
      data-bs-target="#addStudent"
    >
      Adicionar novo aluno
    </button>

    <!-- Modal add student -->
    <div
      class="modal fade"
      id="addStudent"
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
              Adicionar novo aluno
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
                  v-model="newStudent.name"
                />
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="date"
                  class="form-control transparent"
                  aria-describedby="basic-addon1"
                  v-model="newStudent.born_date"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerStudent()"
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

    <!-- Table with students -->
    <div class="overflow">
      <div class="container">
        <div class="row">
          <div class="col-md-12">
            <table class="table">
              <thead>
                <tr>
                  <th>Matrícula</th>
                  <th>Nome</th>
                  <th>Data de nascimento</th>
                  <th style="text-align: right">Ação</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="student in allStudents">
                  <td>{{ student.registry }}</td>
                  <td>{{ student.name }}</td>
                  <td>{{ moment(student.born_date).format("DD/MM/YYYY") }}</td>
                  <td style="text-align: right">
                    <button
                      class="btn btn-outline-warning"
                      v-on:click="selectStudent(student)"
                      data-bs-toggle="modal"
                      data-bs-target="#alterStudent"
                    >
                      Editar
                    </button>
                    <button
                      class="btn btn-outline-danger"
                      v-on:click="deleteStudent(student.id)"
                    >
                      Excluir
                    </button>
                    <button
                      class="btn btn-outline-info"
                      v-on:click="selectStudent(student)"
                      data-bs-toggle="modal"
                      data-bs-target="#menuSelectStudent"
                    >
                      Selecionar
                    </button>
                  </td>
                </tr>
              </tbody>
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
.overflow {
  overflow-x: auto;
}
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
  name: "StudentsComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    await this.getEverything();
  },
  data() {
    return {
      currentLogged: {},
      allStudents: [],
      studentClasses: [],
      allCourses: [],
      currentStudent: undefined,
      allClasses: [],
      newStudent: {
        name: "",
        born_date: "",
        fk_institution: -1,
      },
      newerStudent: {
        name: "",
        born_date: "",
        fk_institution: -1,
      },
      updateStudentClass: 0,
      newStudentClassCourse: 0,
      newStudentClassClass: 0,
      resultMessage: "",
    };
  },
  methods: {
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async addStudentToClass() {
      await api
        .post("/StudentsClasses", {
          fk_student: this.currentStudent,
          fk_class: this.newStudentClassClass,
        })
        .catch(async (err) => {
          this.resultMessage = await err.response.data;
          await this.showMessage();
        });

      await this.getEverything();
    },
    async getEverything() {
      await this.getAllClasses();
      await this.getAllCourses();
      await this.getAllStudent();
      await this.getAllStudentClasses();
    },
    async deleteStudentClasses(id) {
      await api.delete(`/StudentsClasses/${id}`);
      await this.getEverything();
    },
    async saveStudentChanges(id, newClass) {
      await api.put("/StudentsClasses", {
        id: id,
        fk_student: this.currentStudent,
        fk_class: newClass,
      });
      await this.getEverything();
    },
    async selectStudent(student) {
      this.currentStudent = await student.id;
      this.newerStudent.name = await student.name;
      this.newerStudent.born_date = await student.born_date;
    },

    async getAllStudent() {
      this.allStudents = (
        await api.get(
          `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllStudentClasses() {
      this.studentClasses = (
        await api.get(
          `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllClasses() {
      this.allClasses = await (
        await api.get(
          `/Classes/Institution?Institution=${this.currentLogged.fk_institution}`
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
    async registerStudent() {
      await api.post("/Students", {
        name: this.newStudent.name,
        born_date: this.newStudent.born_date,
        fk_institution: this.currentLogged.fk_institution,
      });
      await this.getEverything();
    },
    async deleteStudent(id) {
      await api.delete(`/Students/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data.text;
        await this.showMessage();
      });

      await this.getEverything();
    },
    async alterStudent() {
      await api.put(`/Students`, {
        id: this.currentStudent,
        name: this.newerStudent.name,
        born_date: this.newerStudent.born_date,
        fk_institution: this.currentLogged.fk_institution,
      });
      await this.getEverything();
      this.currentStudent = -1;
    },
  },
};
</script>
