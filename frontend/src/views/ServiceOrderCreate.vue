<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useForm } from 'vee-validate';
import { toTypedSchema } from '@vee-validate/yup';
import * as yup from 'yup';
import { useQuery } from '@tanstack/vue-query';

import { ordersService } from '@/api/modules/ordersService';
import BaseInput from '@/components/inputs/BaseInput.vue';
import BaseTextarea from '@/components/inputs/BaseTextarea.vue';
import BaseChecklist from '@/components/inputs/BaseChecklist.vue';
import BaseFileUpload from '@/components/inputs/BaseFileUpload.vue';

const router = useRouter();

const { data: availableServices, isLoading: isLoadingChecklist } = useQuery({
  queryKey: ['checklist-items'],
  queryFn: ordersService.getChecklistItems,
  staleTime: 1000 * 60 * 30,
});

const schema = toTypedSchema(
  yup.object({
    title: yup.string().required('O título é obrigatório'),
    description: yup.string().required('A descrição é obrigatória').max(250),
    
    checklistItemIds: yup
      .array()
      .of(yup.number())
      .min(1, 'Selecione pelo menos um serviço')
      .required(),

    orderImage: yup
      .mixed()
      // .nullable()
      .test('fileSize', 'O arquivo deve ter no máximo 5MB', (value: any) => {
        if (!value || (Array.isArray(value) && value.length === 0)) return true;
        const file = Array.isArray(value) ? value[0] : value;
        return file.size <= 5000000; 
      }),
  })
);

const { handleSubmit, isSubmitting } = useForm({
  validationSchema: schema,
  initialValues: {
    title: '',
    description: '',
    checklistItemIds: [],
    orderImage: '', 
  },
});

const onSubmit = handleSubmit(async (values) => {
  try {
    const newOrder = await ordersService.create({
      title: values.title,
      description: values.description,
      checklistItemIds: values.checklistItemIds as number[],
    });

    const rawImageValue = values.orderImage;

    if (rawImageValue) {
      let fileToUpload: File | null = null;

      if (Array.isArray(rawImageValue) && rawImageValue.length > 0) {
        fileToUpload = rawImageValue[0];
      } 
      else if (!Array.isArray(rawImageValue) && (rawImageValue as any).size) {
        fileToUpload = rawImageValue as unknown as File;
      }

      if (fileToUpload instanceof File) {
        await ordersService.uploadImage(newOrder.id, fileToUpload);
      }
    }

    alert('Ordem de Serviço criada com sucesso!');
    router.push('/orders');

  } catch (error) {
    console.error('Erro ao criar OS:', error);
    alert('Erro ao salvar. Verifique o console.');
  }
});
</script>

<template>
  <v-container>
    <div class="mb-6">
      <h1 class="text-h5 text-primary font-weight-bold">Nova Ordem de Serviço</h1>
    </div>

    <v-card class="pa-6 rounded-lg" elevation="2">
      <form @submit.prevent="onSubmit">
        
        <BaseInput 
          name="title" 
          label="Título da OS" 
          placeholder="Ex: Falha na ventoinha"
          has-clean-button
        />

        <BaseTextarea 
          name="description" 
          label="Descrição do Problema" 
          placeholder="Descreva detalhadamente..."
          :rows="4"
        />

        <BaseFileUpload 
          name="orderImage"
          label="Foto do Equipamento (Opcional)"
          accept="image/*"
          prepend-icon="mdi-camera"
        />

        <v-divider class="my-6"></v-divider>

        <v-progress-linear 
          v-if="isLoadingChecklist" 
          indeterminate 
          class="mb-4" 
        />
        
        <BaseChecklist 
          name="checklistItemIds" 
          label="Itens de Checklist (Serviços)" 
          :items="availableServices || []" 
        />

        <div class="d-flex justify-end ga-4 mt-6">
          <v-btn 
            variant="text" 
            color="grey-darken-1" 
            to="/orders"
            :disabled="isSubmitting"
          >
            Cancelar
          </v-btn>

          <v-btn 
            type="submit" 
            color="primary" 
            :loading="isSubmitting"
            elevation="2"
          >
            Criar OS
          </v-btn>
        </div>

      </form>
    </v-card>
  </v-container>
</template>