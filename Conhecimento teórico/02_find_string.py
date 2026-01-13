text = input('texto: ')
target = input('alvo: ')

first_world = target[0]
find = False

for i in range(len(text)):
  if(text[i] == first_world):
    if(target == text[i:i+len(target)]):
      print(i)
      find = True;
      break;

if not find:
  print(-1)