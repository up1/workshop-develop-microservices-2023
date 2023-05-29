pipeline {
    agent { label 'docker' }

    stages {
        stage('1. Pull code') {
            steps {
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
                sh '''docker compose -f docker-compose-build.yml up -d stock
                    sleep 5'''
                sh "docker compose -f docker-compose-testing.yml up stock_testing --force-recreate || exit 0"
            }
        }
        stage('4. Testing Pricing service') {
            steps {
                sh '''docker compose -f docker-compose-build.yml up -d pricing
                    sleep 5'''
                sh "docker compose -f docker-compose-testing.yml up pricing_testing --force-recreate --exit-code-from pricing_testing"
            }
        }
        stage('5. Testing Catalog service') {
            steps {
                sh '''docker compose -f docker-compose-build.yml up -d gateway
                    sleep 5
                    sh initial_data.sh
                    sleep 5'''
                sh "docker compose -f docker-compose-testing.yml up catalog_testing --force-recreate --exit-code-from catalog_testing"
            }
        }
        stage('6. Gateway Testing') {
            steps {
                sh 'docker compose -f docker-compose-testing.yml up gateway_testing --force-recreate --exit-code-from gateway_testing'
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
