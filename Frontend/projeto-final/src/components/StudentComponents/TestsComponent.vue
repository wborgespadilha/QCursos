<script setup>
import api from "../../services/api";
import moment from "moment";
</script>

<template>
  <main>
    <h1 class="title">Suas avaliações</h1>
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
            <th>Avaliação</th>
            <th>Data de aplicação</th>
            <th>Tipo de Avaliação</th>
            <th>Nota</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="test in allTests.filter(
              (t) => t.course_id == selectedCourse
            )"
          >
            <td>{{ test.test_name }}</td>
            <td>{{ moment(test.test_date).format("DD/MM/YYYY") }}</td>
            <td>{{ test.testtype_name }}</td>
            <td>{{ test.grade }}</td>
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
      classesList: [],
      currentLogged: {},
      allTests: [],
      allCourses: [],
      selectedCourse: undefined,
    };
  },
  methods: {
    async getEverything() {
      const allTests = api.get(
        `/AppliedTests/Student?student=${this.currentLogged.id}`
      );
      const allCourses = api.get(
        `/Courses/Student?student=${this.currentLogged.id}`
      );

      this.allTests = (await allTests).data;
      this.allCourses = (await allCourses).data;
    },
  },
};
</script>
