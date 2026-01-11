<script setup lang="ts">
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import { toTypedSchema } from '@vee-validate/yup';
import BaseInput from '@/components/inputs/BaseInput.vue';
import { useRouter } from 'vue-router';
import { authService } from '@/api/modules/authService';

const router = useRouter();

const schema = toTypedSchema(
  yup.object({
    email: yup.string().required('Email é obrigatório').email('Email inválido'),
    password: yup.string().required('Senha é obrigatória'),
  })
);

const { handleSubmit, isSubmitting, setErrors } = useForm({
  validationSchema: schema,
});

const onSubmit = handleSubmit(async (values) => {
  try {
    const response = await authService.login(values);
    
    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('userName', response.userName);
    
    router.push('/orders');
  } catch (error: any) {
    setErrors({ email: 'Email ou senha inválidos' });
    console.error(error);
  }
});

const onlyLettersFormat = {
  replace: { pattern: /[^a-zA-Z]/g, to: '' }
};
</script>

<template>
  <v-card width="400" class="pa-6" elevation="3">
    <h2 class="text-center mb-6">Login</h2>

    <form @submit.prevent="onSubmit">
  
  <BaseInput 
    name="email" 
    label="E-mail" 
    placeholder="exemplo@email.com"
    has-clean-button 
  />

  <BaseInput 
    name="password" 
    label="Senha" 
    type="password" 
  />

  <v-btn 
    type="submit" 
    block 
    color="primary" 
    size="large" 
    class="mt-4 text-none"
    :loading="isSubmitting"
  >
    Entrar
  </v-btn>

</form>
  </v-card>
</template>