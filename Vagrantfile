Vagrant.configure("2") do |config|
  require 'yaml'
  settings = YAML.load_file 'vagrantSettings.yaml'

  config.vm.box = 'digital_ocean'
  config.vm.box_url = "https://github.com/devopsgroup-io/vagrant-digitalocean/raw/master/box/digital_ocean.box"
  config.ssh.private_key_path = '~/.ssh/devops'
  
  config.vm.define "droplet1", primary: true do |server|

    server.vm.provider :digital_ocean do |provider|
      provider.ssh_key_name = "devops"
      provider.token = settings["provider_token"]
      provider.image = 'ubuntu-20-04-x64'
      provider.region = 'fra1'
      provider.size = 's-2vcpu-4gb'
    end

    config.vm.synced_folder "remote_files", "/minitwit", type: "rsync"
    config.vm.synced_folder '.', '/vagrant', disabled: true

    server.vm.hostname = "minitwit-ci-server"

    server.vm.provision "shell", inline: 'echo "export DOCKER_USERNAME=' + "'" + settings["DOCKER_USERNAME"] + "'" + '" >> ~/.bash_profile'
    server.vm.provision "shell", inline: 'echo "export DOCKER_PASSWORD=' + "'" + settings["DOCKER_PASSWORD"] + "'" + '" >> ~/.bash_profile'
    
    server.vm.provision "shell",env:{"BASEURL" => settings["BASEURL"],"provider_token" => settings['provider_token'] }, inline: <<-SHELL
    cd ~
    wget https://github.com/digitalocean/doctl/releases/download/v1.92.0/doctl-1.92.0-linux-amd64.tar.gz
    tar xf ~/doctl-1.92.0-linux-amd64.tar.gz
    sudo mv ~/doctl /usr/local/bin
    echo "finished installing doctl"

    echo "token for doctl: $provider_token"

    echo "grant account access to doctl"
    doctl auth init -t $provider_token
    echo "finished granting account access to doctl"

    echo "assign reserved ip to droplet"
    dropletId="$(doctl compute droplet get minitwit-ci-server --template {{.ID}})"

    doctl compute reserved-ip-action assign 104.248.101.163 $dropletId

    echo -e "Setting up environment variables ...\n"
    cd /minitwit
    echo "BASEURL=$BASEURL" >> .env
    echo "CERT_RESOLVER=production" >> .env
    cd ~
    echo -e "Done setting up environment variables ...\n"
    
    # Install docker and docker-compose
    sudo apt install -y apt-transport-https ca-certificates curl software-properties-common
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
    sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu focal stable"
    apt-cache policy docker-ce
    sudo apt install -y docker-ce
    sudo systemctl status docker
    sudo usermod -aG docker ${USER}
    sudo apt install -y docker-compose
    

    # Install make
    sudo apt-get install -y make
    
    echo -e "\nVerifying that docker works ...\n"
    docker run --rm hello-world
    docker rmi hello-world

    echo -e "\nOpening port for minitwit ...\n"
    ufw allow 3000 && \
    ufw allow 22/tcp

    echo ". $HOME/.bashrc" >> $HOME/.bash_profile

    echo -e "\nConfiguring credentials as environment variables...\n"

    source $HOME/.bash_profile

    echo -e "\nSelecting Minitwit Folder as default folder when you ssh into the server...\n"
    echo "cd /minitwit" >> ~/.bash_profile

    chmod +x /minitwit/deploy.sh

    echo -e "\nVagrant setup done ..."
    echo -e "minitwit will later be accessible at http://$(hostname -I | awk '{print $1}'):3000"
    echo -e "The mysql database needs a minute to initialize, if the landing page is stack-trace ..."
    SHELL
  end

  config.vm.define "droplet2", primary: true do |server|
    server.vm.provider :digital_ocean do |provider|
      provider.ssh_key_name = "devops"
      provider.token = settings["provider_token"]
      provider.image = 'ubuntu-20-04-x64'
      provider.region = 'fra1'
      provider.size = 's-2vcpu-4gb'
    end

    config.vm.synced_folder "remote_files_staging", "/minitwit", type: "rsync"
    config.vm.synced_folder '.', '/vagrant', disabled: true

    server.vm.hostname = "minitwit-staging"

    server.vm.provision "shell", inline: 'echo "export DOCKER_USERNAME=' + "'" + settings["DOCKER_USERNAME"] + "'" + '" >> ~/.bash_profile'
    server.vm.provision "shell", inline: 'echo "export DOCKER_PASSWORD=' + "'" + settings["DOCKER_PASSWORD"] + "'" + '" >> ~/.bash_profile'
    
    server.vm.provision "shell",env:{"BASEURL" => settings["BASEURL"],"provider_token" => settings['provider_token'] }, inline: <<-SHELL
    cd ~
    wget https://github.com/digitalocean/doctl/releases/download/v1.92.0/doctl-1.92.0-linux-amd64.tar.gz
    tar xf ~/doctl-1.92.0-linux-amd64.tar.gz
    sudo mv ~/doctl /usr/local/bin
    echo "finished installing doctl"

    echo "token for doctl: $provider_token"

    echo "grant account access to doctl"
    doctl auth init -t $provider_token
    echo "finished granting account access to doctl"

    echo "assign reserved ip to droplet"
    dropletId="$(doctl compute droplet get minitwit-staging --template {{.ID}})"

    doctl compute reserved-ip-action assign 146.190.206.71 $dropletId

    echo -e "Setting up environment variables ...\n"
    cd /minitwit
    echo "BASEURL=$BASEURL" >> .env
    echo "CERT_RESOLVER=production" >> .env
    cd ~
    echo -e "Done setting up environment variables ...\n"
    
    # Install docker and docker-compose
    sudo apt install -y apt-transport-https ca-certificates curl software-properties-common
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
    sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu focal stable"
    apt-cache policy docker-ce
    sudo apt install -y docker-ce
    sudo systemctl status docker
    sudo usermod -aG docker ${USER}
    sudo apt install -y docker-compose
    

    # Install make
    sudo apt-get install -y make
    
    echo -e "\nVerifying that docker works ...\n"
    docker run --rm hello-world
    docker rmi hello-world

    echo -e "\nOpening port for minitwit ...\n"
    ufw allow 3000 && \
    ufw allow 22/tcp

    echo ". $HOME/.bashrc" >> $HOME/.bash_profile

    echo -e "\nConfiguring credentials as environment variables...\n"

    source $HOME/.bash_profile

    echo -e "\nSelecting Minitwit Folder as default folder when you ssh into the server...\n"
    echo "cd /minitwit" >> ~/.bash_profile

    chmod +x /minitwit/deploy.sh

    echo -e "\nVagrant setup done ..."
    echo -e "minitwit will later be accessible at http://$(hostname -I | awk '{print $1}'):3000"
    echo -e "The mysql database needs a minute to initialize, if the landing page is stack-trace ..."
    SHELL
  end

  
end