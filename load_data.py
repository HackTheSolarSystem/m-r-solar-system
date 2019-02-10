#!/usr/bin/env python
# coding: utf-8

# In[1]:


import csv, json, math, copy
import pandas as pd

load = pd.read_csv('data.csv')
print(load)


# In[2]:


columns = ['planet', 'key', 'value', 'value~']
attributes = ['core', 'atmosphere', 'mass', 'distance']
fin_dict = dict()

for item in list(load[columns[0]]): # the planets
    fin_dict[item] = dict()
for index, row in load.iterrows():
    fin_dict[row[columns[0]]][row[columns[1]]]=[]
for index, row in load.iterrows():
    if row[columns[1]]==attributes[1]:
        if math.isnan(row[columns[3]])==False:
            val = {row[columns[2]]:row[columns[3]]}
        else:
            val = {row[columns[2]]:''}
    else:
        val = row[columns[2]]
    fin_dict[row[columns[0]]][row[columns[1]]].append(val)
    


# In[3]:


print(fin_dict) #this is what should go to App to display in "answer search" mode


# # Questions
# 
# ## Core or Atmosphere
# ### Inclusion: "Which of these is in [planet]'s [core/atmosphere]?"
# Generate two false from set of metals/gasses, include one from data
# 
# ### Exclusion: "Which of these is NOT in [planet]'s [core/atmosphere]?"
# Generate one false from set of metals/gasses, include two from data
# 
# ## Distance
# ### Distance (planet unknown): "Which planet is [8.32 light minutes] from the sun?"
# Generate two false planets from set of planets, include ans
# 
# ### Distance (planet known): "How far away is [planet] from the sun?"
# Generate two false distances, include ans
# 
# ## Mass
# ### Mass (planet unknown): "Which planet is [0.815] times the mass of the earth?"
# Generate two false planets, include ans
# 
# ### Mass (planet known): "What is [planet]'s mass?"
# Generate two false masses, include ans

# In[4]:

# TODO: add false attributes to initial CSV load to make completely blind of attribute type (to make fully recyclable)
false = { attributes[0]: ['Gold', 'Silver', 'Aluminum'],
attributes[1] : ['Xenon', 'Radon', 'Chlorine', 'Flourine'], 
attributes[3]  : ['7,047 light minutes', '2 million light minutes', '1.2 light minutes', '5.01527 light minutes'],
attributes[2] : ['200 Me', '6.89 Me', '12.2 Me', '.0001 Me']}

dict_with_false = copy.deepcopy(fin_dict)
for i in list(dict_with_false.keys()): #planets
    for j in list(dict_with_false[i].keys()): #core, atmos, dist, mass
        new_key = j + '_false'
        dict_with_false[i][new_key] = false[j]
        
print(dict_with_false)

print(fin_dict)


# In[87]:


question_types = ['core_in', 'core_ex', 'atmosphere_in', 'atmosphere_ex', 'distance_k','distance_uk', 'mass_u', 'mass_uk']
# question = base:'', inputs = []
# answers = display:[], answer:''

bases = {'core_in': 'Which of these is in {}\'s core?',          'core_ex': 'Which of these is NOT in {}\'s core?',          'atmosphere_in': 'Which of these is in {}\'s atmosphere?',          'atmosphere_ex': 'Which of these is NOT in {}\'s atmosphere?', #          'atmosphere_per': 'What percent of {}\'s atmophere is {}?',\
         'distance_k': 'How far away is {} from the sun?', \
         'distance_uk': 'Which planet is {} from the sun?', \
         'mass_k': 'What is {}\'s mass?',\
         'mass_uk': 'Which planet has mass {}?'
        }

planets = list(fin_dict.keys())
distances = []
masses = []
atmospheres = [] #includes percentages
gasses = [] #doesn't
cores = []

for planet in planets:
    distances.append(fin_dict[planet][attributes[3]][0])
    masses.append(fin_dict[planet][attributes[2]][0])
    atmospheres.append([planet, fin_dict[planet][attributes[1]]])
    cores.append(fin_dict[planet][attributes[0]])

# for tup in atmospheres:
#     gas_list = tup[1]
#     for gas in gas_list:
#         gasses.append(list(gas.keys())[0])
# gasses = set(gasses)
    
# print(gasses)

inputs = {'core_in': planets,          'core_ex': planets,          'atmosphere_in': planets,          'atmosphere_ex': planets, #          'atmosphere_per': atmospheres, \
         'distance_k': planets, \
         'distance_uk': distances, \
         'mass_k': planets,\
         'mass_uk': masses
         }

outputs = {'core_in': attributes[0],          'core_ex': attributes[0],          'atmosphere_in': attributes[1],          'atmosphere_ex': attributes[1], #          'atmosphere_per': attributes[1], \
         'distance_k': attributes[3], \
         'distance_uk': columns[0], \
         'mass_k': attributes[2],\
         'mass_uk': columns[0]
         }

question_pieces = dict()
all_questions = dict()
for key in list(bases.keys()): #question types
    if key not in list(all_questions.keys()):
        all_questions[key] = []
    question_pieces[key] = {'base':bases[key]}
    question_pieces[key]['input'] = inputs[key]
    question_pieces[key]['output'] = outputs[key]
    question_pieces[key]['questions'] = []
    #for the different variations of what can be inputted, create a question for each:
    if key[-2:]=='uk':
        output_type = question_pieces[key]['output']
        for item in question_pieces[key]['input']:
            question = question_pieces[key]['base'].format(item)
            choices = []
            for k in list(dict_with_false.keys()):
                if dict_with_false[k][key[:-3]][0] == item:
                    ans = k
                    false_options = [i for i in planets if i!=ans]
                    for i in false_options:
                        for j in false_options:
                            if i!=j:
                                choice = [ans, i, j]
                                choices.append(choice)
                                question_dict = {'question':question, 'options': choice, 'answer': ans}
                                all_questions[key].append(question_dict)             
    else:
        for item in question_pieces[key]['input']:
#             if type(item) == list:
#                 pass
#                 gas_options = []
#                 for dic in item[1]:
#                     gas_options.append(list(dic.keys()))
#                 for gas in gas_options:
#                     question = question_pieces[key]['base'].format(item[0], gas)
            if type(item) == str:
                question = question_pieces[key]['base'].format(item)
                output_type = question_pieces[key]['output']
                ans_options = dict_with_false[item][output_type]
                false_options = dict_with_false[item][output_type+'_false']
                if key[-2:] == 'ex':
                    num_false = 1
                    one_set = false_options
                    two_set = ans_options
                else:
                    num_false = 2
                    one_set = ans_options
                    two_set = false_options
                choices = []
                for ans in one_set:
                    for i in two_set:
                        for j in two_set:
                            if i!=j:
                                choice = [ans, i, j]
                                if key == 'atmosphere_in' or key == 'atmosphere_ex':
                                    copy_choice = [ans,i,j]
                                    for c in choice:
                                        if type(c)==dict:
                                            copy_choice.remove(c)
                                            copy_choice.append(list(c.keys())[0])
                                    choice = copy_choice
                                question_dict = {'question':question, 'options': choice, 'answer': ans}
                                all_questions[key].append(question_dict) 
                                choices.append(choice)

                question_pieces[key]['questions'].append(question)


print(all_questions)
with open('data.json', 'w') as outfile:
    json.dump(all_questions, outfile)
print('done')




