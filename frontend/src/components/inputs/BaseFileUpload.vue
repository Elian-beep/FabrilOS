<script setup lang="ts">
import { useField } from 'vee-validate';
import { toRef } from 'vue';

interface Props {
  name: string;
  label?: string;
  accept?: string;
  prependIcon?: string;
}

const props = withDefaults(defineProps<Props>(), {
  label: 'Selecione um arquivo',
  accept: 'image/*',
  prependIcon: 'mdi-camera',
});

const nameRef = toRef(props, 'name');
const { value, errorMessage, handleBlur, handleChange } = useField<File[] | null>(nameRef);
</script>

<template>
  <div class="mb-4">
    <v-file-input
      v-model="value"
      :id="name"
      :error-messages="errorMessage"
      :label="label"
      :accept="accept"
      :prepend-icon="prependIcon"
      variant="outlined"
      color="primary"
      show-size
      counter
      clearable
      @blur="handleBlur"
      @update:model-value="handleChange"
    ></v-file-input>
  </div>
</template>