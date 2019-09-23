const autocomplete = async (animalUrl, searchDiv, resultsDiv) => {
    const search = searchDiv;
    const results = resultsDiv;
    const ul = document.createElement('ul');
    let animals;
    let filteredAnimals = {};
    let search_term = '';
    let selectedItemId = -1;
    let selectedAnimal = {};

    const getAnimals = async (url) => {
        animals = await fetch(url).then(response => response.json());
    };

    const showAnimals = async () => {
        results.innerHTML = '';
        ul.innerHTML = '';
        ul.classList.add('animals');
        selectedAnimal = undefined;

        filteredAnimals
            .forEach(animal => {
                const li = document.createElement('li');
                li.classList.add('animal-item');

                const animal_name = document.createElement('h6');

                let startOfSearchTerm = animal.UniqueAnimalId.toLowerCase().indexOf(search_term);
                let endOfSearchTerm = startOfSearchTerm + search_term.length;
                let formattedText = animal.UniqueAnimalId.substring(0, startOfSearchTerm) +
                    '<strong><u>' + animal.UniqueAnimalId.substring(startOfSearchTerm, endOfSearchTerm) + '</u></strong>' +
                    animal.UniqueAnimalId.substring(endOfSearchTerm);

                animal_name.innerHTML = search_term === '' ? animal.UniqueAnimalId : formattedText;
                animal_name.classList.add('animal-name');
                li.appendChild(animal_name);

                if (animal.HasPicture) {
                    const animal_image = document.createElement('img');
                    animal_image.src = '/Content/AnimalImages/' + animal.Id + '.jpg';
                    animal_image.classList.add('animal-picture');
                    li.appendChild(animal_image);
                }

                li.addEventListener('click', function (e) {
                    selectedAnimal = animal;
                    submitForm();
                });

                ul.appendChild(li);
            });

        if (filteredAnimals.length !== 0) {
            selectedItemId = 0;
            ul.childNodes[0].classList.add('selected');
            selectedAnimal = filteredAnimals[0];
            results.appendChild(ul);
        }
    };

    const updateActive = () => {
        selectedAnimal = filteredAnimals[selectedItemId];

        if (selectedAnimal === undefined) {
            return;
        }

        search.value = selectedAnimal.UniqueAnimalId;
        ul.childNodes.forEach(item => {
            item.classList.remove('selected');
        });

        let animalItem = ul.childNodes[selectedItemId];

        animalItem.classList.add('selected');
        animalItem.scrollTop = animalItem.scrollHeight;
    };

    const submitForm = () => {
        let idToPost = document.createElement('input');
        idToPost.type = 'hidden';
        idToPost.value = selectedAnimal.Id;
        idToPost.name = 'animalid';
        results.append(idToPost);
        document.forms[1].submit();
    };

    search.addEventListener('input', e => {
        search_term = e.target.value;
        filteredAnimals = animals.filter(animal =>
            animal.UniqueAnimalId.toLowerCase().includes(search_term.toLowerCase())
        );
        showAnimals();
    });

    search.addEventListener('keydown', e => {
        //if (filteredAnimals === undefined || filteredAnimals.length === 0) {
        //    return;
        //}

        switch (e.keyCode) {
            case 40: // Down
                e.preventDefault();
                if (selectedItemId === filteredAnimals.length - 1) return;
                selectedItemId++;
                updateActive();
                resultsDiv.scrollTop += 50;
                break;
            case 38: // Up
                e.preventDefault();
                if (selectedItemId === 0) return;
                selectedItemId--;
                updateActive();
                resultsDiv.scrollTop -= 50;
                break;
            case 13: // return
                e.preventDefault();
                if (selectedAnimal !== undefined) submitForm();
        }

    });

    await getAnimals(animalUrl);
};