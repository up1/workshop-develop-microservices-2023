pipeline {
    agent { label 'docker' }

    stages {
        stage('1. Pull code') {
            steps {
                // checkout scm
                git branch: 'main', url: 'https://github.com/up1/workshop-develop-microservices-2023.git'
            }
        }
        stage('2. Build image') {
            steps {
                sh "docker compose -f docker-compose-build.yml build"
            }
        }
        stage('3. Testing Stock service') {
            steps {
                sh '''docker compose -f docker-compose-build.yml up -d mysql
                    sleep 5'''
                sh '''docker compose -f docker-compose-build.yml up -d stock
                    sleep 5'''
                sh "docker compose -f docker-compose-testing.yml up stock_testing --abort-on-container-exit --build"
            }
        }
        stage('4. Testing Pricing service') {
            steps {
                sh '''docker compose -f docker-compose-build.yml up -d pricing
                    sleep 5'''
                sh "docker compose -f docker-compose-testing.yml up pricing_testing --abort-on-container-exit --build"
            }
        }
        stage('5. Testing Catalog service') {
            steps {
                sh '''docker compose -f docker-compose-build.yml up -d database
                    sleep 5
                    docker compose -f docker-compose-build.yml up -d catalog
                    sleep 5
                    curl http://134.209.105.128:9999/init'''
                sh "docker compose -f docker-compose-testing.yml up catalog_testing --abort-on-container-exit --build"
            }
        }
        stage('6. Gateway Testing') {
            steps {
                sh 'docker compose -f docker-compose-build.yml up -d gateway'
                sh 'sleep 5'
                sh 'docker compose -f docker-compose-testing.yml up gateway_testing --abort-on-container-exit --build'
            }
        }
    }
    post {
        always {
            sh 'docker compose -f docker-compose-build.yml down'
            sh 'docker compose -f docker-compose-testing.yml down'
            sh 'docker volume prune -f'
        }
    }
}
