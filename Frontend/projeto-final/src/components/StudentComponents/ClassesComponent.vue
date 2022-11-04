<script setup>
import api from "../../services/api";
import avatar from "animal-avatar-generator";
import "animate.css";
</script>

<template>
  <main>
    <h1 class="title">Turmas</h1>
    <hr />

    <!-- Accordion with clases and classmates -->
    <div class="accordion accordion-flush bg-dark" id="accordionFlushExample">
      <div class="accordion-item bg-dark" v-for="_class in allClasses">
        <h2 class="accordion-header bg-dark" id="flush-headingOne">
          <button
            class="accordion-button collapsed bg-dark"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="
              '#class_' + _class.name.replaceAll(' ', '_').replaceAll('\'', '_')
            "
            aria-expanded="false"
            aria-controls="flush-collapseOne"
          >
            {{ _class.course_name }} - {{ _class.name }}
          </button>
        </h2>
        <div
          :id="
            'class_' + _class.name.replaceAll(' ', '_').replaceAll('\'', '_')
          "
          class="accordion-collapse collapse bg-dark"
          aria-labelledby="flush-headingOne"
          data-bs-parent="#accordionFlushExample"
        >
          <div class="accordion-body bg-dark">
            <h3>Professor: {{ _class.teacher_name }}</h3>
            <hr />
            <div class="row">
              <div
                v-for="classmate in allClassmates.filter(
                  (c) => c.class_id == _class.id
                )"
                class="col-xxl-3 col-lg-4 col-md-6 col-sm-12 col-12 col"
              >
                <div class="card animate__animated animate__fadeIn">
                  <div
                    class="avatar-container"
                    v-html="generateAvatar(classmate.id)"
                  ></div>
                  <h4 class="student-card-name">{{ classmate.name }}</h4>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
main {
  /* border: 1px solid red; */
  margin-top: 25px;
}
.avatar-container {
  display: inline-block;
  width: 50%;
  margin: 0 auto;
}
.student-card-name {
  display: inline-block;
  margin-top: 1rem;
  font-weight: 600;
}
.row {
  display: flex;
  justify-content: space-evenly;
}
.card {
  border-radius: 10px;
  background-color: #292929;
  padding: 1rem;
  text-align: center;
}
.title {
  font-weight: 600;
  color: #fff;
}
.td {
  /* border: 1px solid #fff; */
  padding: 10px 5px;
  font-weight: 600;
}
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
</style>

<script>
export default {
  name: "ClassesComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );

    await this.getAllClasses();
    await this.getAllStudents();
    await this.getAllStudentClasses();
    await this.getAllClassmates();
  },
  data() {
    return {
      currentLogged: {},
      allClasses: [],
      allStudents: [],
      allStudentClasses: [],
      allClassmates: [],

      image_string: "",
    };
  },
  methods: {
    async getAllClasses() {
      this.allClasses = (
        await api.get(`/Classes/Student?Student=${this.currentLogged.id}`)
      ).data;
    },
    async getAllStudents() {
      this.allStudents = (
        await api.get(
          `/Students/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    async getAllClassmates() {
      this.allClassmates = (
        await api.get(`/Students/Student?Student=${this.currentLogged.id}`)
      ).data;
    },
    async getAllStudentClasses() {
      this.allStudentClasses = (
        await api.get(
          `/StudentsClasses/Institution?Institution=${this.currentLogged.fk_institution}`
        )
      ).data;
    },
    generateAvatar(id) {
      return avatar(`${id}`, { size: 90 });
    },
  },
};
</script>
