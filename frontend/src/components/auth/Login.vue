<template>
    <div class="form-signin">
        <form method="POST" v-on:submit.prevent="loginUser">
            <font-awesome-icon :icon="['fas', 'gamepad']" class="logo"/>
            <h1 class="h3 mb-3 fw-normal">Please login</h1>
            <div class="form-floating">
                <input type="text" class="form-control" id="floatingInput" placeholder="username"
                       v-model="loginData.username">
                <label for="floatingInput">Username</label>
            </div>
            <div class="form-floating">
                <input type="password" class="form-control" id="floatingRepeatPassword" placeholder="Password"
                       v-model="loginData.passwd">
                <label for="floatingRepeatPassword">Password</label>
            </div>
            <div class="checkbox mb-3">
                <label>
                    <router-link to="/register">Register here!</router-link>
                </label>
            </div>
            <p :key="err" v-for="err in loginData.error">{{ err }}</p>
            <button class="w-100 btn btn-lg btn-primary" type="submit">Login</button>
        </form>
    </div>
</template>

<script lang="ts">
import * as Log from '../../domain/auth/login';
import {defineComponent} from 'vue';

export default defineComponent({
    name: 'Login',
    props: ['userName'],
    emits: ['fetchUserName'],
    data: function () {
        return {
            loginData: new Log.LoginData() as Log.ILoginData,
        }
    },
    methods: {
        loginUser: async function () {
            this.loginData.error = new Array<string>();
            this.loginData.fieldsNotEmpty();
            if (this.loginData.error.length > 0) {
                return;
            }
            let user: Log.IUser = new Log.User(
                this.loginData.username,
                this.loginData.passwd
            );
            let status = await user.loginUserRequest();
            if (status === 401) {
                this.loginData.error.push("Username or password is wrong");
                return;
            }
            if (status === 200) {
                this.$emit('fetchUserName', this.loginData.username);
                this.loginData = new Log.LoginData();

                await this.$router.push('/');
            }
        },
    }
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>


</style>
