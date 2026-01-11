<script setup lang="ts">
import { useField } from 'vee-validate';
import { toRef, ref, watch, onUnmounted } from 'vue';

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

const previewUrl = ref<string | null>(null);

watch(value, (newFiles) => {
  if (previewUrl.value) {
    URL.revokeObjectURL(previewUrl.value);
    previewUrl.value = null;
  }

  if (newFiles && newFiles.length > 0) {
    const file = newFiles[0];
    if (file && file.type.startsWith('image/')) {
      previewUrl.value = URL.createObjectURL(file);
    }
  }
});

onUnmounted(() => {
  if (previewUrl.value) URL.revokeObjectURL(previewUrl.value);
});
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