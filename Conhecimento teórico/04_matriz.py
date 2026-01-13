import random

def generate_matriz(a, b):
  matriz = []
  for i in range(a):
      current_a = []
      
      for j in range(b):
          numero_aleatorio = random.randint(0, 100) 
          current_a.append(numero_aleatorio)
      
      matriz.append(current_a)
  
  return matriz

def max_in_matriz():
  mat = generate_matriz(2, 2)
  for i in mat:
    print(i)
  max = mat[0][0]

  for i in mat:
    for j in i:
      if(j>max):
         max = j;
  print(max)

max_in_matriz()