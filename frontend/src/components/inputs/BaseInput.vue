<script setup lang="ts">
import { useField } from 'vee-validate';
import { computed, ref, toRef } from 'vue';

interface Props {
  name: string;
  label?: string;
  type?: 'text' | 'password' | 'email';
  placeholder?: string;
  hasCleanButton?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  type: 'text',
  hasCleanButton: false,
});

const nameRef = toRef(props, 'name');
const { value, errorMessage, handleBlur } = useField(nameRef);

const showPassword = ref(false);
const isPasswordType = computed(() => props.type === 'password');

const currentType = computed(() => {
  if (isPasswordType.value) return showPassword.value ? 'text' : 'password';
  return props.type;
});

const clearInput = () => {
  value.value = '';
};
</script>

<template>
  <div class="mb-4">
    <v-text-field
      v-model="value"
      :id="name"
      :error-messages="errorMessage"
      :label="label"
      :placeholder="placeholder"
      :type="currentType"
      variant="outlined"
      color="primary"
      density="comfortable"
      hide-details="auto"
      @blur="handleBlur"
    >
      <template v-if="isPasswordType" #append-inner>
        <v-icon
          style="cursor: pointer"
          @click="showPassword = !showPassword"
          color="grey-darken-1"
        >
          {{ showPassword ? 'mdi-eye-off' : 'mdi-eye' }}
        </v-icon>
      </template>

      <template v-else-if="hasCleanButton && value" #append-inner>
         <v-icon 
            style="cursor: pointer" 
            @click="clearInput"
            color="grey-darken-1"
          >
            mdi-close
         </v-icon>
      </template>
    </v-text-field>
  </div>
</template>