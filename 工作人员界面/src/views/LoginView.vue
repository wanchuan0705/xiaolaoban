<template>
    <div class="login-box">
        <el-form ref="ruleFormRef" :model="ruleForm" status-icon :rules="rules" label-width="80px" class="demo-ruleForm">
            <h2>交通治超执法管理系统</h2>
            <el-form-item label="账号：" prop="username">
                <el-input v-model="ruleForm.username" autocomplete="off" />
            </el-form-item>

            <el-form-item label="密码：" prop="password">
                <el-input v-model="ruleForm.password" type="password" autocomplete="off" />
            </el-form-item>
            <el-form-item>
                <el-button class="loginBtn" type="primary" @click="submitForm(ruleFormRef)">登录</el-button>
                <el-button class="loginBtn" @click="resetForm">重置</el-button>
            </el-form-item>
        </el-form>
    </div>
</template>

<script lang="ts">
import { ElMessage, ElMessageBox, FormInstance } from 'element-plus';
import type { Action } from 'element-plus'
import { defineComponent, reactive, ref, toRefs } from 'vue'
import { LoginData } from "../type/login"
// import { login } from "../request/api";
import axios from 'axios';
import { useRouter } from 'vue-router';
export default defineComponent({
    setup() {
        const data = reactive(new LoginData());
        const rules = {
            username: [
                {
                    required: true,
                    message: "请输入你的账号",
                    trigger: "blur",
                },
                {
                    min: 3,
                    max: 10,
                    message: "账号的长度在3到10之间",
                    trigger: "blur",
                },
            ],
            password: [
                {
                    required: true,
                    message: "请输入你的密码",
                    trigger: "blur",
                },
                {
                    min: 3,
                    max: 10,
                    message: "密码的长度在3到10之间",
                    trigger: "blur",
                },
            ],
        };
        //登录
        const ruleFormRef = ref<FormInstance>();
        const router = useRouter();
        const submitForm = async (formEl: FormInstance | undefined) => {
            if (!formEl) return;
            else {
                // 对表单的内容进行验证
                // valid布尔类型，为true表示验证成功，反之
                formEl.validate(async (valid) => {
                    if (valid) {
                        const response = await axios.post('http://localhost:5172/api/Admin/Login', null, {
                            params: {
                                username: data.ruleForm.username,
                                password: data.ruleForm.password
                            }
                        });

                        // 处理响应
                        console.log(response.data); // 假设响应返回数据

                        if (response.status === 200) {
                            router.push('/')
                        }
                        else {
                            ElMessageBox.alert('账号或密码错误', '登录提示', {
                                // if you want to disable its autofocus
                                // autofocus: false,
                                confirmButtonText: '确认',
                                callback: (action: Action) => {
                                    ElMessage({
                                        type: 'info',
                                        message: `action: ${action}`,
                                    })
                                },
                            })
                        }
                    } else {
                        console.log("error submit!");
                        return false;
                    }
                });
                console.log(formEl);
            }
        };
        // 重置
        const resetForm = () => {
            data.ruleForm.username = "";
            data.ruleForm.password = "";
        };
        return { ...toRefs(data), rules, resetForm, ruleFormRef, submitForm }
    }
})
</script>

<style lang="scss" scoped>
.login-box {
    width: 100%;
    height: 100%;
    background: url("../assets/bg.jpg");
    padding: 1px;
    text-align: center;

    .demo-ruleForm {
        width: 500px;
        margin: 450px auto;
        background-color: #fff;
        padding: 40px;
        border-radius: 20px;
    }

    .loginBtn {
        width: 48%;
    }

    h2 {
        margin-bottom: 10px;
    }
}
</style>