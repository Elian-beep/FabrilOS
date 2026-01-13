first_array = [1, 3, 22, 5, 16]
second_array = [7, 4, 1, 8, 11]

third_array = first_array + second_array
final_array = []

for i in range(len(third_array)):
  isSwitched = False
  for j in range(0, len(third_array)-i-1):
    if third_array[j] > third_array[j + 1]:
      temp = third_array[j]
      third_array[j] = third_array[j + 1]
      third_array[j + 1] = temp
      swapped = True
  if not swapped:
    break

print(third_array)