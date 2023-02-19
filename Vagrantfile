Vagrant.configure('2') do |config|

  config.vm.define "droplet1" do |config|
      config.vm.provider :digital_ocean do |provider, override|
        override.ssh.private_key_path = '~/.ssh/devops'
        override.vm.box = 'digital_ocean'
        override.vm.box_url = "https://github.com/devopsgroup-io/vagrant-digitalocean/raw/master/box/digital_ocean.box"
        override.nfs.functional = false
        override.vm.allowed_synced_folder_types = :rsync
        provider.token = 'dop_v1_d7c18ed7f8dd2f4033e12b2b39f55aa0a70c4f963504cab39e7fcd53318dcd69'
        provider.image = 'ubuntu-18-04-x64'
        provider.region = 'fra1' # Frankfurt - physical location
        provider.size = 's-2vcpu-4gb' # 2 vCPU, 4GB RAM - mssql needs 2gb ram
        provider.backups_enabled = false
        provider.private_networking = false
        provider.ipv6 = false
        provider.monitoring = false
      end

      config.vm.synced_folder ".", "/vagrant", disabled: true

      config.vm.provision "shell", inline: <<-SHELL
        sudo apt-get update -qq -y
        echo "starting docker install"
      SHELL
      config.vagrant.plugins = "vagrant-docker-compose"
      # install docker and docker-compose
      config.vm.provision :docker
      config.vm.provision :docker_compose

      config.vm.provision "shell", inline: <<-SHELL
        echo "finished docker install"
      SHELL

      config.vm.provision "shell", inline: <<-SHELL
        echo "starting git clone"
        git clone https://github.com/Dev-Janitors/minitwit.git
        echo "finished git clone"

        echo "starting docker-compose"
        cd minitwit

        docker-compose up -d --build
        echo "finished docker-compose"
      
      SHELL

      #config.vm.provision "file", source: "./", destination: "$HOME/minitwit/"

      #config.vm.provision "file", source: "./Database", destination: "$HOME/minitwit/Database"
      # config.vm.provision "shell", inline: <<-SHELL
      #   echo "copied Database"
      
      #   SHELL
      # config.vm.provision "file", source: "./Frontend", destination: "$HOME/minitwit/Frontend"
      # config.vm.provision "shell", inline: <<-SHELL
      #   echo "copied Frontend"
      
      #   SHELL
      # config.vm.provision "file", source: "./Backend", destination: "$HOME/minitwit/Backend"
      # config.vm.provision "shell", inline: <<-SHELL
      #   echo "copied Backend"
      
      #   SHELL

  end
end