<script setup>
import api from "../../services/api";
import moment from "moment";
</script>

<template>
  <main>
    <h1 class="title">Suas presenças</h1>
    <hr />
    <!-- Course selector -->
    <select class="form-select transparent" v-model="selectedCourse">
      <option class="text-dark" v-for="course in allCourses" :value="course.id">
        {{ course.name }}
      </option>
    </select>

    <!-- Table with tests -->
    <div class="overflow">
      <table class="table">
        <thead>
          <tr>
            <th>Turma</th>
            <th>Data</th>
            <th>Presença</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="attendance in allAttendances.filter( a => a.course_id == selectedCourse)">
            <td>{{attendance.class_name}}</td>
            <td>{{moment(attendance.lesson_date).format('DD/MM/YYYY')}}</td>
            <td>{{attendance.absence == 0 ? "Presente" : (attendance.absence == 1) ? "Faltou" : "Faltou (Justificado)"}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>

<style scoped>
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
.transparent {
  color: #fff !important;
  background-color: transparent;
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
      allAttendances: [],
      allCourses: [],
      selectedCourse: undefined,
    };
  },
  methods: {
    async getEverything() {
      this.allAttendances = (await api.get(`/Attendances/Student?Student=${this.currentLogged.id}`)).data;
      this.allCourses = (await api.get(`/Courses/Student?student=${this.currentLogged.id}`)).data;
    },
  },
};
</script>