import { defineStore } from "pinia";
import { ref } from "vue";

export const loginStore = defineStore('login', () =>{
    const policeId = ref(0);
    const roleId = ref(0);
    const password = ref("");
    const policeName =ref("")
    return { policeName,password,roleId,policeId };
})