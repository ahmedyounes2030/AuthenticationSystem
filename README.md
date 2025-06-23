graph LR
    A[Presentation] -->|Uses| B[Application]
    B -->|Depends On| C[Domain]
    B -->|Depends On| D[Infrastructure]
    D -->|Implements| C[Domain Interfaces]
    D -->|Persists| E[(Database)]
    
    subgraph Clean Architecture
        C[Domain]
        B[Application]
        A[Presentation]
        D[Infrastructure]
    end
