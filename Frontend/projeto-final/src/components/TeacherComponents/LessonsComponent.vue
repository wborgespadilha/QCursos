<script setup>
import api from "../../services/api";
import moment from "moment";
</script>

<template>
  <main>
    <h1 class="title">Suas aulas</h1>
    <hr />

    <!-- Modal add lesson -->
    <div
      class="modal fade"
      id="addAttendanceModal"
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
              Adicionar nova aula
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <form>
              <div class="input-group mb-3">
                <span class="input-group-text" id="basic-addon1">🔖</span>
                <input
                  type="date"
                  class="form-control transparent"
                  aria-describedby="basic-addon1"
                  v-model="newLesson.date"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer bg-dark">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-outline-success"
              v-on:click="registerLesson()"
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

    <!-- Modal select lesson -->
    <div
      class="modal fade"
      id="selectAttendanceModal"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content bg-dark">
          <div class="modal-header bg-dark">
            <h5 class="modal-title" id="staticBackdropLabel">
              Adicionar presenças
            </h5>
          </div>
          <div class="modal-body bg-dark">
            <table class="table table-striped">
              <thead>
                <th>Matrícula</th>
                <th>Aluno</th>
                <th>Presença</th>
              </thead>
              <tbody>
                <tr v-for="(attendance, pos) in allAttendances">
                  <td>{{allStudents.find(s => s.id == attendance.fk_student).registry}}</td>
                  <td>{{allStudents.find(s => s.id == attendance.fk_student).name}}</td>
                  <td>
                    <select v-model="allAttendances[pos].absence" class="form-select transparent">
                        <option value=0 :selected="allAttendances[pos].absence == 0">Presente</option>
                        <option value=1 :selected="allAttendances[pos].absence == 1">Faltou</option>
                        <option value=2 :selected="allAttendances[pos].absence == 2">Faltou (Justificado)</option>
                    </select>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="modal-footer bg-dark">
            <button type="button" class="btn btn-outline-success" v-on:click="saveStudentPresence()">
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
    <div class="row">
      <div class="col">
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
        <button
          type="button"
          class="btn btn-outline-primary"
          data-bs-toggle="modal"
          data-bs-target="#addAttendanceModal"
          style="width: 100%"
        >
          Adicionar nova aula
        </button>
      </div>
    </div>

    <hr />

    <!-- Table with lessons -->
    <table class="table">
      <thead>
        <tr>
          <th>Data</th>
          <th style="text-align: right">Ação</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="lesson in allLessons.filter((l) => l.fk_class == currentClass)"
        >
          <td>{{ moment(lesson.lesson_date).format("DD/MM/YYYY") }}</td>
          <td style="text-align: right">
            <button
              class="btn btn-outline-info"
              data-bs-toggle="modal"
              data-bs-target="#selectAttendanceModal"
              v-on:click="selectLesson(lesson.id)"
              style="margin: 0 5px"
            >
              Selecionar
            </button>
            <button
              class="btn btn-outline-danger"
              v-on:click="deleteLesson(lesson.id)"
              style="margin: 0 5px"
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
.selectModel {
  color: transparent !important;
}
.selectModel:focus {
  color: #fff !important;
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
  name: "LessonsComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );
    await this.getEverything();
  },
  data() {
    return {
      currentLogged: {},
      allLessons: [],
      allClasses: [],
      allCourses: [],
      allStudents: [],
      allStudentsClasses: [],
      allAttendances: [],

      currentCourse: undefined,
      currentClass: undefined,
      currentLesson: undefined,
      studentPresence: undefined,

      newLesson: {
        date: undefined,
      },

      resultMessage: "",
    };
  },
  methods: {
    async selectLesson(id) {
      this.currentLesson = await id;
      await this.getAllAttendances(this.currentLesson);
    },
    async showMessage() {
      const resultToast = document.getElementById("resultToast");
      const toast = new bootstrap.Toast(resultToast);
      toast.show();
    },
    async getEverything() {
      const lessons = await api.get(
        `/Lessons/Teacher?Teacher=${this.currentLogged.id}`
      );
      const classes = await api.get(
        `/Classes/Teacher?Teacher=${this.currentLogged.id}`
      );
      const courses = await api.get(
        `/Courses/Institution?institution=${this.currentLogged.fk_institution}`
      );
      const students = await api.get(
        `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
      );
      const studentsClasses = await api.get(
        `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
      );

      this.allLessons = await lessons.data;
      this.allClasses = await classes.data;
      this.allCourses = await courses.data;
      this.allStudents = await students.data;
      this.allStudentsClasses = await studentsClasses.data;
    },
    async getAllAttendances(id) {
      const attendances = await api.get(
        `/Attendances/Teacher?Teacher=${this.currentLogged.id}`
      );
      this.allAttendances = attendances.data.filter(a => a.fk_lesson == id);
    },
    async registerLesson() {
      await api.post("/Lessons", {
        lesson_date: this.newLesson.date,
        fk_class: this.currentClass,
      });
      await this.getEverything();
    },
    async deleteLesson(id) {
      await api.delete(`/Lessons/${id}`).catch(async (err) => {
        this.resultMessage = await err.response.data.text;
        await this.showMessage();
      });
      await this.getEverything();
    },
    async saveStudentPresence(attendance, value) {
      const response = await api.put("/Attendances", this.allAttendances);

      this.resultMessage = await response.data;
      await this.showMessage();

      await this.getEverything();
    },
  },
};
</script>
