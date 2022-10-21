<template>
    <div style="width:750px;" class="text-center p-5">
        <h1 class="m-3">Lobby</h1>
        <h6 class="text-start">With id: {{ lobby.id }}</h6>
        <h4 v-if="lobby.userNames" class="text-start">Game host: {{ lobby.userNames[0] }}</h4>
        <h5 v-if="lobby.gameType === 2" class="text-start">
            Proposer is played by: {{ proposer }}
        </h5>
        <table class="table table-striped shadow align-middle">
            <thead>
            <tr>
                <th scope="col" class="text-start">#</th>
                <th scope="col" class="text-start">Player</th>
            </tr>
            </thead>
            <tbody v-if="lobby.userNames">
            <tr v-for="(player, index) in lobby.userNames" :key="index">
                <th scope="row" class="text-start">{{ index + 1 }}</th>
                <td class="text-start">{{ player }}</td>
            </tr>
            </tbody>
        </table>
        <button
            v-if="lobby.userNames && lobby.userNames[0] == userName && lobbyChoices.gametype == 2"
            class="btn btn-lg w-100 btn-primary rounded-3 shadow-lg my-3"
            v-on:click="startGame"
        >
            Start
        </button>
        <button v-on:click="quitLobby"
                class="btn btn-lg w-100 btn-danger rounded-3 shadow-lg my-3"
        >
            Leave Lobby
        </button>

    </div>
</template>

<script lang="ts">
import {defineComponent, PropType} from "vue";
import * as Lob from "./../../domain/game/Lobby";

export default defineComponent({
    name: "Lobby",
    emits: ["startGame", "joinStartedGame", "leftLobby"],
    props: {
        userName: {
            type: String,
            required: true
        },
        lobbyChoices: {
            type: Object as PropType<Lob.LobbyChoices>,
            required: true
        }
    },
    data: function () {
        return {
            lobby: new Object() as Lob.Lobby,
            lobbyStatus: new Object() as Lob.LobbyStatus,
            polling: 0,
            proposer: "",
        };
    },

    methods: {
        quitLobby: async function () {
            let response = await fetch("/Lobby/Quit");
            if (response.status == 200) {
                this.$emit("leftLobby");

            }
        },
        getLobby: async function () {
            let response = await Lob.getLobby();
            if (response.status === 200) {
                this.lobby = response.body.data;
                if (this.lobby.userNames.length == 2 && this.lobbyChoices.role == 1 && this.lobby.gameType == 1) {
                    this.startGame();
                }
                if (this.lobby.gameId > 0) {
                    this.$emit("joinStartedGame");
                }
            } else {
                clearInterval(this.polling);
            }
        },
        pollLobby: async function () {
            this.polling = setInterval(async () => {
                await this.getLobby();
            }, 1000);
        },

        startGame: function () {
            this.$emit("startGame");
        },
        setProposer: function () {
            if (this.lobby.gameType === 2 && this.lobby.hostRole === 0) {
                this.proposer = "oracle";
            } else if (this.lobby.gameType === 2 && this.lobby.hostRole === 1) {
                this.proposer = this.lobby.userNames[0];
            }
        }
    },
    beforeDestroy() {
        clearInterval(this.polling);
    },

    created: async function () {
        await this.getLobby();
        await this.pollLobby();
        this.setProposer();
    }
});
</script>
