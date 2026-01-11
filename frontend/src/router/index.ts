import { createRouter, createWebHistory } from 'vue-router';

import Login from '@/views/Login.vue';
import ServiceOrderList from '@/views/ServiceOrderList.vue';
import ServiceOrderCreate from '@/views/ServiceOrderCreate.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: Login,
      meta: { requiresAuth: false },
    },
    {
      path: '/',
      redirect: '/orders',
    },
    {
      path: '/orders',
      name: 'orders-list',
      component: ServiceOrderList,
      meta: { requiresAuth: true },
    },
    {
      path: '/orders/new',
      name: 'orders-create',
      component: ServiceOrderCreate,
      meta: { requiresAuth: true },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const requiresAuth = to.meta.requiresAuth;

  const isAuthenticated = localStorage.getItem('token');

  if (requiresAuth && !isAuthenticated) {
    next('/login');
  } else if (to.path === '/login' && isAuthenticated) {
    next('/orders');
  } else {
    next();
  }
});

export default router;
