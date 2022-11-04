<script setup>
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import api from "../../services/api";
</script>

<template>
  <main>
    <h1 class="title">Atestados</h1>
    <hr />

    <div class="row">
      <div v-for="_class in allClasses" class="col-md-6">
        <div class="card">
          <h4 class="class-card-name">{{ _class.course_name }}</h4>
          <button class="btn btn-outline-primary" v-on:click="generatePdf(_class.id)">
            Gerar PDF
          </button>
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
.col-md-6 {
  margin: 1rem 0;
}
.class-card-name {
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
hr {
  height: 1px;
  background-color: #fff;
  border: none;
}
</style>

<script>
export default {
  name: "CertificatesComponent",
  async beforeMount() {
    this.currentLogged = await JSON.parse(
      localStorage.getItem("@q-cursos:user")
    );

    await this.getEverything();
    this.institution = (await api.get("/Institution")).data.find(
      (i) => i.id == this.currentLogged.fk_institution
    );
  },
  data() {
    return {
      currentLogged: {},
      allAverages: [],
      allClasses: [],
      institution: {},
    };
  },
  methods: {
    async getEverything() {
      const response = await api.get(
        `/Classes/Student?Student=${this.currentLogged.id}`
      );
      this.allClasses = response.data;
    },
  },
};
</script>
