# BackendWebApiCRUDColumnsOrder

Descripción del Repositorio.
=============================
WebAPI Template, que contiene funciones y métodos necesarios para hacer un CRUD con un modelo de Entidad, además permite hacer un get utilizando paginación y ordenamiento por columna.



Tecnologías utilizadas: 
------------------------

-   Framework de desarrollo: .Net 5, Con librerías de clases con Net Standar 2.1

-   Lenguaje Programación C\#.

-   Servicios utlizados: WebAPI Rest.

-   Frameworks para la utilización de datos: Entity Framework Core.

-   Visualización de datos: JSON utilizando el paquete o librería
    Newtonsoft.Json.

-	Mapeo de Clases por medio de la Librería AutoMapper    

-   Deploy del Backend: Si se usa Windows, ocupar \"WebAPI\" y no IIS,
    ya que IIS no funcionará con el frontend o habría que realizar
    cambios en la URL base del front.

-   Acceso a Base de datos: Se utilizó un archivo de SQLite para
    generarción de datos, se ecuentra en WebApi/storage (Se Crea a partir
    del uso de migraciones o parte del codigo). 

Requisitos:
-----------

-   Tener instalado .Net 5.0.5 SDK 5.0.202, puede ser obtenido desde:
    [https://dotnet.microsoft.com/download/dotnet/5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

-   Algún IDE que pueda ejecutar el código en .Net 5, con sus
    respectivos paquetes Nuget, pueden ser Visual Studio 2019 o Rider 2020+.

-   Opcional: Tener en cuenta para ejecutar el Script, necesita Entity
    Framework Core.

Estructura del repositorio:
---------------------------

El repositorio consta de varias capas o proyectos en si los cuales se
detallarán a continuación:

-   **WebApi**: es el proyecto principal del repositorio, en el cual se
    interactúa con las peticiones que realice el usuario, es donde se
    encuentran los controladores o los diferentes sub-sitios para que se
    llamen a las distintas clases que se tienen el proyecto, consta de
    la siguiente estructura:

    -   *Properties*: carpeta donde se almacenan los archivos de
        configuración de propiedades para la iniciación de servicios.

    -   *Controllers*: carpeta que contiene a los controladores donde el
        usuario o sistemas puede hacer las peticiones para traspasar
        datos o recibir datos.

    -   *Extensions*: carpeta que contiene las extensiones de las clases
        que configuran el backend, contiene los métodos necesarios para
        que pueda funcionar correctamente.

    -   *Utils*: carpeta que contiene las clases de utilidad
        para poder ser ocupadas en los diferentes métodos de este
        proyecto.

    -   *Helpers*: carpeta que contiene las clases de ayuda a entidades
        para poder ser ocupadas en los diferentes métodos de este
        proyecto.

    -   *Helpers*: carpeta donde nuestro archivo binario de base de 
    	datos estará alojado

    -   *Principal, Raíz o WebApi*: carpeta que contiene las clases que
        inician los servicios, como también las clases que llaman a
        métodos para configurar estos servicios, además contiene un
        archivo json para almacenar los diferentes datos que son de
        petición global al repositorio.

-   **BusinessRules**: es el proyecto que contiene toda la lógica de los
    negocios, es la cual ayuda a las peticiones a que ser realicen
    correctamente, utilizando diferentes métodos para hacer de manera
    satisfactoria las diferentes transacciones entre las capas, esta
    divido en los diferentes módulos que se ocuparan en los sistemas o
    aplicaciones, consta de la siguiente estructura:

	-	*Entities*: Carpeta donde se contienen los servicios o reglas
		de negocios de las entidades.

-   **Entities**: es el proyecto que contiene todas las clases
    necesarias para la interacción con la base de datos, es la capa
    donde se crean las diferentes clases que crean las entidades
    necesarias para obtener datos de la base de datos. Este proyecto
    consta de la siguiente estructura:

    -   *Models*: carpeta que contiene todas las clases que son
        entidades para la base de datos.

    -   *Extensions*: carpeta que contiene todas las extensiones de las
        clases con los métodos que se ocuparan en el CRUD con la base de
        datos.

    -   *Utils*: carpeta que contiene las clases de utilidad que se 
    	ocuparan para la ayuda de la visualización de datos.

    -   *Helpers*: carpeta que contiene las clases de ayuda a las entidades
    	para la obtención, modificación o creación de datos.

    -   *Principal, Raíz o Entities*: carpeta que contiene todas las
        carpetas mencionadas anteriormente, además de la interfaz
        IEntity, la cual es la base para poder crear las clases e
        implementar el Id de todas ellas, y la clase RepositoryContext,
        es la que ayuda a EntityFramework a que se relacione todas las
        clases de *Models* con la Base de datos.

-   **Contracts**: es el proyecto que contiene todas las interfaces que
    van a ser implementadas por las diferentes clases del proyecto
    **Repository**. En este proyecto se definen las interfaces con los
    diferentes prototipos de métodos, también es una ayuda para poder
    crear la inyección de dependencias para que sean llamadas por los
    métodos de las clases de la lógica de negocios y así que sean un
    medio de interacción no directa entre negocios y datos, además al
    igual que **Entities** y **BusinessRules**,:

    -	*Entities*: Carpeta donde se contienen las interfaces para los
    	repositorios de las entidades.

    -   *Interfaces*: carpeta que contiene las interfaces con los
        prototipos de métodos base para ser implementadas por las clases
        correspondientes de **Repository**.

-   **Repository:** Es el proyecto que contiene todas las clases que se
    interactúan con la base de datos, la cual además se crean las
    consultas, implementan las interfaces del proyecto **Contracts** e
    implementa Entity Framework Core para poder realizar el Mapeo
    necesario para que enlacen con las distintas entidades/clases que se
    definen en **Entities**:

    -   *Base*: Carpeta que contiene la clase base, que será ocupada por
        las diferentes clases compuestas, además tiene los métodos CRUD
        generales para las entidades puedan interactuar con la base de
        datos.

    -	*Entities*: Carpeta donde se contienen los repositorios de las 
    	entidades, utilizando los metodos base para el CRUD.

    -   *Utils*: Carpeta que contiene las clases que serán de ayuda para
        interactuar con las clases de Repositorio, está presente una
        extensión que interactúa para la paginación de los resultados.

    -   *Wrappers*: Carpeta que contiene contenedores de repositorios,
        esta permite la ayuda entre el lazo de la capa de Negocios con
        la de Datos, además permite la ayuda de la implementación de
        inyección de dependencias y que cualquier repositorio, pueda
        tener una relación directa con la entidad definida.


LICENCIA.
==========
Este Template contiene la licencia MIT.
