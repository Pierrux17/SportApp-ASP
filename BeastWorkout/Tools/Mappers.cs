using BeastWorkout.Models;
using DAL.Entities;

namespace BeastWorkout.Tools
{
    public static class Mappers
    {
        //---------------COUNTRY-------------

        public static Country ToASP(this CountryDAL c)
        {
            return new Country()
            {
                Id = c.Id,
                Name = c.Name,
            };
        }

        public static CountryDAL ToDAL(this Country c)
        {
            return new CountryDAL()
            {
                Id = c.Id,
                Name = c.Name,
            };
        }

        //---------------TYPEPERSON-------------
        public static TypePerson ToASP(this TypePersonDAL t)
        {
            return new TypePerson()
            {
                Id = t.Id,
                Name = t.Name,
            };
        }

        public static TypePersonDAL ToDAL(this TypePerson t)
        {
            return new TypePersonDAL()
            {
                Id = t.Id,
                Name = t.Name,
            };
        }

        //---------------PERSON-------------
        public static Person ToASP(this PersonDAL p) 
        {
            return new Person()
            {
                Id = p.Id,
                Lastname = p.Lastname,
                Firstname = p.Firstname,
                Mail = p.Mail,
                Login = p.Login,
                Password = p.Password,
                Password_reset_token = p.Password_reset_token,
                Auth_key = p.Auth_key,
                Created_at = p.Created_at,
                Updated_at = p.Updated_at,
                Is_validate = p.Is_validate,
                Id_type_person = p.Id_type_person,
                Id_country = p.Id_country,
            };
        }

        public static PersonDAL ToDAL(this Person p)
        {
            return new PersonDAL()
            {
                Id = p.Id,
                Lastname = p.Lastname,
                Firstname = p.Firstname,
                Mail = p.Mail,
                Login = p.Login,
                Password = p.Password,
                Password_reset_token = p.Password_reset_token,
                Auth_key = p.Auth_key,
                Created_at = p.Created_at,
                Updated_at = p.Updated_at,
                Is_validate = p.Is_validate,
                Id_type_person = p.Id_type_person,
                Id_country = p.Id_country,
            };
        }


        //---------------PROGRAM-------------
        public static BeastWorkout.Models.Program ToASP(this ProgramDAL p)
        {
            return new BeastWorkout.Models.Program()
            {
                Id = p.Id,
                Name = p.Name,
                Nbtrainingperweek = p.Nbtrainingperweek,
                Duration = p.Duration,
                Objectif = p.Objectif,
                Id_type_program = p.Id_type_program,
                Is_my_Program = p.Is_my_Program,
                Created_by = p.Created_by,
            };
        }

        public static ProgramDAL ToDAL(this BeastWorkout.Models.Program p)
        {
            return new ProgramDAL()
            {
                Id = p.Id,
                Name = p.Name,
                Nbtrainingperweek = p.Nbtrainingperweek,
                Duration = p.Duration,
                Objectif = p.Objectif,
                Id_type_program = p.Id_type_program,
                Is_my_Program = p.Is_my_Program,
                Created_by = p.Created_by,
            };
        }

        //---------------TYPEPROGRAM-------------
        public static TypeProgram ToASP(this TypeProgramDAL t)
        {
            return new TypeProgram()
            {
                Id = t.Id,
                Name = t.Name,
                Picture = t.Picture,
            };
        }

        public static TypeProgramDAL ToDAL(this TypeProgram t)
        {
            return new TypeProgramDAL()
            {
                Id = t.Id,
                Name = t.Name,
                Picture = t.Picture,
            };  
        }

        //---------------TRAINING-------------
        public static Training ToASP(this TrainingDAL t)
        {
            return new Training()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Picture = t.Picture,
            };
        }

        public static TrainingDAL ToDAL(this Training t)
        {
            return new TrainingDAL()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Picture = t.Picture,
            };
        }

        //---------------EXERCICE-------------
        public static Exercice ToASP(this ExerciceDAL e)
        {
            return new Exercice()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Picture = e.Picture,
                Id_type_exercice = e.Id_type_exercice,
            };
        }

        public static ExerciceDAL ToDAL(this Exercice e)
        {
            return new ExerciceDAL()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Picture = e.Picture,
                Id_type_exercice = e.Id_type_exercice,
            };
        }

        //---------------SORTEXERCICE-------------
        public static SortExercice ToASP(this SortExerciceDAL s)
        {
            return new SortExercice()
            {
                Id = s.Id,
                Name = s.Name,
                Picture = s.Picture,
            };
        }

        public static SortExerciceDAL ToDAL(this SortExercice s)
        {
            return new SortExerciceDAL()
            {
                Id = s.Id,
                Name = s.Name,
                Picture = s.Picture
            };
        }

        //---------------TYPEEXERCICE-------------
        public static TypeExercice ToASP(this TypeExerciceDAL t)
        {
            return new TypeExercice()
            {
                Id = t.Id,
                Name = t.Name,
                Picture = t.Picture,
                Id_sort_exercice = t.Id_sort_exercice,
            };
        }

        public static TypeExerciceDAL ToDAL(this TypeExercice t)
        {
            return new TypeExerciceDAL()
            {
                Id = t.Id,
                Name = t.Name,
                Picture = t.Picture,
                Id_sort_exercice = t.Id_sort_exercice,
            };
        }

        //---------------PROFIL-------------
        public static ProfilDAL ToDAL(this Profil p)
        {
            return new ProfilDAL()
            {
                Id = p.Id,
                Age = p.Age,
                Height = p.Height,
                Weight = p.Weight,
                Total_xp = p.Total_xp,
                Id_person = p.Id_person,
            };
        }

        public static Profil ToASP(this ProfilDAL p)
        {
            return new Profil()
            {
                Id = p.Id,
                Age = p.Age,
                Height = p.Height,
                Weight = p.Weight,
                Total_xp = p.Total_xp,
                Id_person = p.Id_person,
            };
        }

        //---------------PERSONPROGRAM-------------  --> table d'association
        public static PersonProgram ToASP(this PersonProgramDAL p)
        {
            return new PersonProgram()
            {
                Id_person = p.Id_person,
                Id_program = p.Id_program,
            };
        }

        public static PersonProgramDAL ToDAL(this PersonProgram p)
        {
            return new PersonProgramDAL()
            {
                Id_person = p.Id_person,
                Id_program = p.Id_program,
            };
        }

        //---------------PROGRAMTRAINING-------------  --> table d'association
        public static ProgramTraining ToASP(this ProgramTrainingDAL p)
        {
            return new ProgramTraining()
            {
                Id_program = p.Id_program,
                Id_training = p.Id_training,
            };
        }

        public static ProgramTrainingDAL ToDAL(this ProgramTraining p)
        {
            return new ProgramTrainingDAL()
            {
                Id_program = p.Id_program,
                Id_training = p.Id_training,
            };
        }

        //---------------TRAININGEXERCICE-------------  --> table d'association
        public static TrainingExercice ToASP(this TrainingExerciceDAL t)
        {
            return new TrainingExercice()
            {
                Id_training = t.Id_training,
                Id_exercice = t.Id_exercice,
                Serie = t.Serie,
                Reps = t.Reps,
                Rest = t.Rest,
                Weight = t.Weight,
                Rpe = t.Rpe,
                Distance = t.Distance,
                Time = t.Time,
                Cpt = t.Cpt,
            };
        }

        public static TrainingExerciceDAL ToDAL(this TrainingExercice t)
        {
            return new TrainingExerciceDAL()
            {
                Id_training = t.Id_training,
                Id_exercice = t.Id_exercice,
                Serie = t.Serie,
                Reps = t.Reps,
                Rest = t.Rest,
                Weight = t.Weight,
                Rpe = t.Rpe,
                Distance = t.Distance,
                Time = t.Time,
                Cpt = t.Cpt,
            };
        }

        //---------------LOGIN-------------
        public static LoginForm ToASP(this LoginDAL l)
        {
            return new LoginForm()
            {
                Login = l.Login,
                Password = l.Password,
            };
        }

        public static LoginDAL ToDAL(this LoginForm l)
        {
            return new LoginDAL()
            {
                Login = l.Login,
                Password = l.Password,
            };
        }

        //---------------TRAININGLOG---------------
        public static TrainingLogDAL ToDAL(TrainingLog t)
        {
            return new TrainingLogDAL()
            {
                Id = t.Id,
                Date = t.Date,
                Id_person = t.Id_person,
                Id_training = t.Id_training,
            };
        }

        public static TrainingLog ToASP(TrainingLogDAL t)
        {
            return new TrainingLog()
            {
                Id = t.Id,
                Date = t.Date,
                Id_person = t.Id_person,
                Id_training = t.Id_training,
            };
        }

        //---------------EXERCICELOG---------------
        public static ExerciceLogDAL ToDAL(ExerciceLog e)
        {
            return new ExerciceLogDAL()
            {
                Id = e.Id,
                Reps = e.Reps,
                Weight = e.Weight,
                Distance = e.Distance,
                Time = e.Time,
                Comment = e.Comment,
                Id_training_log = e.Id_training_log,
                Id_exercice = e.Id_exercice,
            };
        }

        public static ExerciceLog ToASP(ExerciceLogDAL e)
        {
            return new ExerciceLog()
            {
                Id = e.Id,
                Reps = e.Reps,
                Weight = e.Weight,
                Distance = e.Distance,
                Time = e.Time,
                Comment = e.Comment,
                Id_training_log = e.Id_training_log,
                Id_exercice = e.Id_exercice,
            };
        }

        //---------------PERFORMANCE---------------
        public static PerformanceDAL ToDAL(Performance p)
        {
            return new PerformanceDAL()
            {
                Id = p.Id,
                Description = p.Description,
                Value = p.Value,
                Date = p.Date,
                Id_profil = p.Id_profil,
                Id_exercice = p.Id_exercice,
            };
        }

        public static Performance ToASP(PerformanceDAL p)
        {
            return new Performance()
            {
                Id = p.Id,
                Description = p.Description,
                Value = p.Value,
                Date = p.Date,
                Id_profil = p.Id_profil,
                Id_exercice = p.Id_exercice,
            };
        }
    }
}
