<template>
    <div class="form-signin">
        <form method="POST" v-on:submit.prevent="registerUser">
            <font-awesome-icon :icon="['fas', 'gamepad']" class="logo"/>
            <h1 class="h3 mb-3 fw-normal">Please register</h1>
            <div class="form-floating">
                <input type="text" class="form-control" id="floatingInput" placeholder="username"
                       v-model="registerData.username" required>
                <label for="floatingInput">Pick a username</label>
            </div>
            <div class="form-floating">
                <input type="password" class="form-control" id="floatingPassword" placeholder="Password"
                       v-model="registerData.password" required>
                <label for="floatingPassword">Password</label>
            </div>
            <div class="form-floating">
                <input type="password" class="form-control" id="floatingRepeatPassword" placeholder="Repeat password"
                       v-model="registerData.repassword" required>
                <label for="floatingRepeatPassword">Repeat password</label>
            </div>
            <p :key="err" v-for="err in registerData.error">{{ err }}</p>
            <button class="w-100 btn btn-lg btn-primary" type="submit">Register</button>
        </form>
    </div>
</template>

<script lang="ts">

import * as Reg from '../../domain/auth/register';
import {defineComponent} from 'vue';

export default defineComponent({
    name: 'Register',
    props: ['userName'],
    emits: ['fetchUserName'],
    data: function () {
        return {
            registerData: new Reg.RegisterData() as Reg.IRegisterData,
        }
    },
    methods: {
        registerUser: async function () {
            this.registerData.passwordRequirementsCheck();
            if (this.registerData.error.length > 0) {
                return
            }
            let user: Reg.IUser = new Reg.User(
                this.registerData.username,
                this.registerData.password
            );
            let response = await user.registerUserRequest();
            if (response.status === 201) {
                this.registerData = new Reg.RegisterData();
                await this.$router.push('/login');
            }
            this.registerData.error = response.body.errors;

        },

    }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>


</style>
