<template>
    <div style="width:750px;" class="text-center p-5">
        <h1 class="m-3">Choose game to join!</h1>
        <table class="table table-striped shadow align-middle">
            <thead>
            <tr>
                <th scope="col" class="text-start">#</th>
                <th scope="col" class="text-start">Players</th>
                <th scope="col" class="text-end"></th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(lobby, index) in lobbies" :key="index">
                <td scope="row" class="text-start">{{ index + 1 }}</td>
                <td class="text-start">
                        <span
                            v-for="(player, index) in lobby.userNames"
                            :key="index"
                        >{{ player }},
                        </span>
                </td>
                <td class="text-end">
                    <button
                        class="btn btn-primary"
                        v-on:click="joinLobby(lobby.id)"
                    >
                        Join
                    </button>
                </td>
            </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
import {defineComponent, PropType} from "vue";
import Lobby from "./Lobby.vue";
import * as Lob from "./../../domain/game/Lobby";

export default defineComponent({
    components: {Lobby},
    name: "Lobbies",
    emits: ["joinLobby", 'getJoinedLobby'],
    props: {
        lobbyChoices: {
            type: Object as PropType<Lob.LobbyChoices>,
            required: true
        },
    },
    data: function () {
        return {
            lobbies: new Object() as Lob.Lobbies,
            polling: 0
        };
    },
    methods: {
        joinLobby: async function (value: number) {
            this.lobbyChoices.lobbyId = value;
            await this.postLobby();
            clearInterval(this.polling);
            this.$emit("getJoinedLobby")

        },
        postLobby: async function () {
            var response = await Lob.postLobby({
                Id: this.lobbyChoices.lobbyId,
                Type: this.lobbyChoices.gametype,
                Role: this.lobbyChoices.role
            })
            if (response.status === 404) {
                this.$router.push('/')
            }
        },
        pollLobbies: async function () {
            this.polling = setInterval(async () => {
                await this.getLobbies();
            }, 3000);
        },
        getLobbies: async function () {
            let response = await Lob.getLobbiesByType(
                this.lobbyChoices.gametype
            );
            if (response.status === 200) {
                this.lobbies = response.body.data;
            }
        },


    },
    beforeDestroy() {
        clearInterval(this.polling);
    },
    created: async function () {
        await this.getLobbies();
        await this.pollLobbies();
    }
});
</script>
