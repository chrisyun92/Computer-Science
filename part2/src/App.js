import React, { useState, useEffect } from 'react'
import axios from 'axios'
import Note from './components/Note'

/*const CourseNameMapper = ({ parts }) => {
    return (parts.map(part =>
        <ul key={part.id}>
            {part.name} {part.exercises}
        </ul>
    ))
}

const CourseExerciseSummer = ({ parts }) => {

    const sum = parts.reduce(
        (sum, currPart) => sum + currPart.exercises, 0
    )

    return (
        <ul>
            <p>total of {sum} exercises</p>
        </ul>
    )
}

const CourseOrganizer = ({ course }) => {
    return (
        <>
            <h1>{course.name}</h1>
            <CourseNameMapper parts={course.parts} />
            <CourseExerciseSummer parts={course.parts} />
        </> 
    )
}

const App = ({ courses }) => {
    return (
        <>
            {courses.map(course =>
                <CourseOrganizer key={course.id} course={course} />
            )}
        </>
    )
}*/

/*const DisplayPerson = ({ person }) => {
    return (
        <ul>
            {person.name}
        </ul>
    )
}

const CreateNameObject = (name, personsArrayLen) => {
    const nameObject = {
        name: name,
        date: new Date().toISOString(),
        id: personsArrayLen + 1,
    }
    return nameObject
}

const App = () => {
    const [persons, setPersons] = useState([{ name: 'Arto Hellas' }])
    const [newName, setNewName] = useState('')



    const handleNameChange = (event) => {
        console.log(event.target.value)
        setNewName(event.target.value)
    }

    const addNewName = (event) => {
        event.preventDefault()
        const nameObject = CreateNameObject(newName, persons.length)
        setPersons(persons.concat(nameObject))
        setNewName('')
    }

    return (
        <div>
            <h2>Phonebook</h2>
            <form onSubmit={addNewName}>
                <div>
                    name: <input value={newName} onChange={handleNameChange} />
                </div>
                <div>
                    <button type="submit">add</button>
                </div>
            </form>
            <h2>Numbers</h2>
            {persons.map(person =>
                <DisplayPerson key={person.id} person={person} />
            )}
    </div>
    )
}*/
const App = (props) => {
    const [notes, setNotes] = useState([])
    const [newNote, setNewNote] = useState('') 
    const [showAll, setShowAll] = useState(true)

    const hook = () => {
        console.log('effect')
        axios
            .get('http://localhost:3001/notes')
            .then(response => {
                console.log('promise fulfilled')
                setNotes(response.data)
            })
    }

    useEffect(hook, [])
    console.log('render', notes.length, 'notes')

    const addNote = (event) => {
        event.preventDefault()
        const noteObject = {
            content: newNote,
            date: new Date().toISOString(),
            important: Math.random() < 0.5,
            id: notes.length + 1,
        }

        setNotes(notes.concat(noteObject))
        setNewNote('')
    }

    const handleNoteChange = (event) => {
        console.log(event.target.value)
        setNewNote(event.target.value)
    }

    const notesToShow = showAll
        ? notes
        : notes.filter(note => note.important)

    return (
        <div>
            <h1>Notes</h1>
            <div>
                <button onClick={() => setShowAll(!showAll)}>
                    show {showAll ? 'important' : 'all'}
                </button>
            </div>
            <ul>
                {notesToShow.map(note =>
                    <Note key={note.id} note={note} />
                )}
            </ul>
            <form onSubmit={addNote}>
                <input
                    value={newNote}
                    onChange={handleNoteChange}
                />
                <button type="submit">save</button>
            </form> 
        </div>
    )
}

export default App 